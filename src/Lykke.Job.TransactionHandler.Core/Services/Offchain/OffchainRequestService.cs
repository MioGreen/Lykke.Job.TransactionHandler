﻿using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;

namespace Lykke.Job.TransactionHandler.Core.Services.Offchain
{
    public interface IOffchainRequestService
    {
        Task CreateOffchainRequest(string transactionId, string clientId, string assetId, decimal amount, string orderId, OffchainTransferType type);

        Task NotifyUser(string clientId);

        Task CreateOffchainRequestAndNotify(string transactionId, string clientId, string assetId, decimal amount, string orderId, OffchainTransferType type);

        Task CreateOffchainRequestAndLock(string transactionId, string clientId, string assetId, decimal amount, string orderId, OffchainTransferType type);

        Task CreateOffchainRequestAndUnlock(string transactionId, string clientId, string assetId, decimal amount, string orderId, OffchainTransferType type);

        Task CreateHubCashoutRequests(string clientId, decimal bitcoinAmount = 0, decimal lkkAmount = 0);
    }
}