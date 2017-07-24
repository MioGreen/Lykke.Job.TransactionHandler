using System;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Lykke.Job.TransactionHandler.Queues.Common
{
    public abstract class RabbitQueue
    {
        private readonly ILog _log;

        protected RabbitQueue(string rabbitHost, int rabbitPort,
            string rabbitExchangeName, string rabbitQueueName,
            string rabbitUsername, string rabbitPassword, ILog log, bool durable = true, bool autoDelete = false)
        {
            _log = log;
            var factory = new ConnectionFactory
            {
                HostName = rabbitHost,
                Port = rabbitPort,
                UserName = rabbitUsername,
                Password = rabbitPassword,

                AutomaticRecoveryEnabled = true
            };

            try
            {
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.BasicQos(0, 1, true);
                channel.ExchangeDeclare(rabbitExchangeName, "fanout", true);
                channel.QueueDeclare(rabbitQueueName, durable, false, autoDelete);
                channel.QueueBind(rabbitQueueName, rabbitExchangeName, "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += MessageReceived;
                channel.BasicConsume(rabbitQueueName, false, consumer);
            }
            catch (Exception ex)
            {
                _log?.WriteErrorAsync(nameof(RabbitQueue), "Creator", "Initializing Connection", ex);
                throw;
            }
        }

        private async void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            var message = string.Empty;
            try
            {
                message = Encoding.UTF8.GetString(ea.Body);
                var processed = await ProcessMessage(message);
                if (processed)
                {
                    ((EventingBasicConsumer)sender).Model.BasicAck(ea.DeliveryTag, false);
                    return;
                }
            }
            catch (Exception ex)
            {
                var writeErrorAsync = _log?.WriteErrorAsync(nameof(RabbitQueue), "MessageReceived", message, ex);
                if (writeErrorAsync != null)
                    await writeErrorAsync;
            }

            ((EventingBasicConsumer)sender).Model.BasicReject(ea.DeliveryTag, false);
            var warning = _log?.WriteWarningAsync(nameof(RabbitQueue), "MessageReceived", message, "Mesage was not processed");
            if (warning != null)
                await warning;
        }

        public abstract Task<bool> ProcessMessage(string message);
    }
}