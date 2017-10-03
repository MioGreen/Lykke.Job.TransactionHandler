using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Common.ApiLibrary.Middleware;
using Lykke.Common.ApiLibrary.Swagger;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Models;
using Lykke.Job.TransactionHandler.Modules;
using Lykke.Job.TransactionHandler.Queues;
using Lykke.JobTriggers.Extenstions;
using Lykke.Logs;
using Lykke.SettingsReader;
using Lykke.SlackNotification.AzureQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Job.TransactionHandler
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IContainer ApplicationContainer { get; set; }
        public IConfigurationRoot Configuration { get; }

        private readonly IEnumerable<Type> _subscribers = new List<Type>
        {
            typeof(CashInOutQueue), typeof(EthereumEventsQueue), typeof(LimitTradeQueue),
            typeof(TradeQueue), typeof(TransferQueue)
        };

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new Newtonsoft.Json.Serialization.DefaultContractResolver();
                });

            services.AddSwaggerGen(options =>
            {
                options.DefaultLykkeConfiguration("v1", "TransactionHandler API");
            });

            var builder = new ContainerBuilder();
            var appSettings = Configuration.LoadSettings<AppSettings>();
            var log = CreateLogWithSlack(services, appSettings);

            builder.RegisterModule(new JobModule(appSettings, log));

            if (string.IsNullOrWhiteSpace(appSettings.CurrentValue.TransactionHandlerJob.Db.BitCoinQueueConnectionString))
            {
                builder.AddTriggers();
            }
            else
            {
                builder.AddTriggers(pool =>
                {
                    pool.AddDefaultConnection(appSettings.CurrentValue.TransactionHandlerJob.Db.BitCoinQueueConnectionString);
                });
            }

            builder.Populate(services);

            ApplicationContainer = builder.Build();

            StartSubscribers();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLykkeMiddleware("TransactionHandler", ex => new ErrorResponse { ErrorMessage = "Technical problem" });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();

            appLifetime.ApplicationStopped.Register(() =>
            {
                StopSubscribers();
                ApplicationContainer.Dispose();
            });
        }

        private static ILog CreateLogWithSlack(IServiceCollection services, IReloadingManager<AppSettings> settings)
        {
            var logToConsole = new LogToConsole();
            var aggregateLogger = new AggregateLogger();

            aggregateLogger.AddLog(logToConsole);

            var slackService = services.UseSlackNotificationsSenderViaAzureQueue(new AzureQueueIntegration.AzureQueueSettings
            {
                ConnectionString = settings.CurrentValue.SlackNotifications.AzureQueue.ConnectionString,
                QueueName = settings.CurrentValue.SlackNotifications.AzureQueue.QueueName
            }, aggregateLogger);

            var dbLogConnectionString = settings.CurrentValue.TransactionHandlerJob.Db.LogsConnString;

            // Creating azure storage logger, which logs own messages to concole log
            if (!string.IsNullOrEmpty(dbLogConnectionString) && !(dbLogConnectionString.StartsWith("${") && dbLogConnectionString.EndsWith("}")))
            {
                const string appName = "Lykke.Job.TransactionHandler";

                var persistenceManager = new LykkeLogToAzureStoragePersistenceManager(
                    appName,
                    AzureTableStorage<LogEntity>.Create(
                        settings.ConnectionString(x => x.TransactionHandlerJob.Db.LogsConnString),
                        "TransactionHandlerLog", logToConsole),
                    logToConsole);

                var slackNotificationsManager = new LykkeLogToAzureSlackNotificationsManager(appName, slackService, logToConsole);

                var azureStorageLogger = new LykkeLogToAzureStorage(
                    appName,
                    persistenceManager,
                    slackNotificationsManager,
                    logToConsole);

                azureStorageLogger.Start();

                aggregateLogger.AddLog(azureStorageLogger);
            }

            return aggregateLogger;
        }

        private void StartSubscribers()
        {
            foreach (var subscriber in _subscribers)
            {
                ((IQueueSubscriber)ApplicationContainer.Resolve(subscriber)).Start();
            }
        }

        private void StopSubscribers()
        {
            foreach (var subscriber in _subscribers)
            {
                ((IQueueSubscriber)ApplicationContainer.Resolve(subscriber)).Stop();
            }
        }
    }
}