using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Queues.Common;
using Lykke.Job.TransactionHandler.Queues.Models;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Queues
{
    public class SwapQueue : RabbitQueue
    {
        private readonly IBitcoinCommandSender _bitcoinCommandSender;
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly IBitCoinTransactionsRepository _bitCoinTransactionsRepository;

        public SwapQueue(AppSettings.RabbitMqSettings config, ILog log,
            IBitcoinCommandSender bitcoinCommandSender,
            IWalletCredentialsRepository walletCredentialsRepository,
            IBitCoinTransactionsRepository bitCoinTransactionsRepository)
            : base(config.ExternalHost, config.Port,
                config.ExchangeSwapOperation, "transactions.swaps",
                config.Username, config.Password, log)
        {
            _bitcoinCommandSender = bitcoinCommandSender;
            _walletCredentialsRepository = walletCredentialsRepository;
            _bitCoinTransactionsRepository = bitCoinTransactionsRepository;
        }

        //ToDo: not tested at all
        public override async Task<bool> ProcessMessage(string message)
        {
            var queueMessage = JsonConvert
                .DeserializeObject<SwapQueueItem>(message);
            var amount1 = queueMessage.Amount1.ParseAnyDouble();
            var amount2 = queueMessage.Amount2.ParseAnyDouble();

            //Get client wallets
            var wallet1 = await _walletCredentialsRepository.GetAsync(queueMessage.ClientId1);
            var wallet2 = await _walletCredentialsRepository.GetAsync(queueMessage.ClientId2);

            //Create and save context data
            var contextData = new DirectSwapContextData
            {
                FirstParty = new DirectSwapContextData.PartyModel
                {
                    ClientId = queueMessage.ClientId1,
                    AssetId = queueMessage.AssetId1,
                    Amount = amount1
                },
                SecondParty = new DirectSwapContextData.PartyModel
                {
                    ClientId = queueMessage.ClientId2,
                    AssetId = queueMessage.AssetId2,
                    Amount = amount2
                }
            };

            var transactionId = Guid.NewGuid();

            await _bitCoinTransactionsRepository.CreateAsync(transactionId.ToString(), BitCoinCommands.Swap,
                "", contextData.ToJson(), "");

            //Send to bitcoin
            await _bitcoinCommandSender.SendCommand(new SwapCommand
            {
                TransactionId = transactionId,
                MultisigCustomer1 = wallet1.MultiSig,
                Asset1 = queueMessage.AssetId1,
                Amount1 = amount1,

                MultisigCustomer2 = wallet2.MultiSig,
                Asset2 = queueMessage.AssetId2,
                Amount2 = amount2,
                
                Context = contextData.ToJson()
            });

            return true;
        }
    }
}