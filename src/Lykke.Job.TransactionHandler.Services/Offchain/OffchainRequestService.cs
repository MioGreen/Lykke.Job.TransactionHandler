﻿using System;
using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Domain.Clients.Core.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;
using Lykke.Job.TransactionHandler.Core.Services.AppNotifications;
using Lykke.Job.TransactionHandler.Core.Services.Offchain;
using Lykke.Service.ClientAccount.Client.AutorestClient;
using Lykke.Service.PendingActions.AutorestClient.Models;
using Lykke.Service.PendingActions.Client;

namespace Lykke.Job.TransactionHandler.Services.Offchain
{
    public class OffchainRequestService : IOffchainRequestService
    {
        private readonly IOffchainRequestRepository _offchainRequestRepository;
        private readonly IOffchainTransferRepository _offchainTransferRepository;
        private readonly IClientSettingsRepository _clientSettingsRepository;
        private readonly IClientAccountService _clientAccountService;
        private readonly IAppNotifications _appNotifications;
        private readonly IPendingActionsClient _pendingActionsClient;

        public OffchainRequestService(IOffchainRequestRepository offchainRequestRepository,
            IOffchainTransferRepository offchainTransferRepository, IClientSettingsRepository clientSettingsRepository,
            IClientAccountService clientAccountService, IAppNotifications appNotifications,
            IPendingActionsClient pendingActionsClient)
        {
            _offchainRequestRepository = offchainRequestRepository;
            _offchainTransferRepository = offchainTransferRepository;
            _clientSettingsRepository = clientSettingsRepository;
            _clientAccountService = clientAccountService;
            _appNotifications = appNotifications;
            _pendingActionsClient = pendingActionsClient;
        }

        public async Task CreateOffchainRequest(string transactionId, string clientId, string assetId, decimal amount, string orderId, OffchainTransferType type)
        {
            await _offchainTransferRepository.CreateTransfer(transactionId, clientId, assetId, amount, type, null, orderId);

            await _offchainRequestRepository.CreateRequest(transactionId, clientId, assetId, RequestType.RequestTransfer, type, null);

            await _pendingActionsClient.SetPendingAsync(clientId, PendingActionType.OffchainRequest, true);
        }

        public async Task NotifyUser(string clientId)
        {
            var pushSettings = await _clientSettingsRepository.GetSettings<PushNotificationsSettings>(clientId);
            if (pushSettings.Enabled)
            {
                var clientAcc = await _clientAccountService.GetByIdAsync(clientId) as Lykke.Service.ClientAccount.Client.AutorestClient.Models.ClientResponseModel;

                await _appNotifications.SendDataNotificationToAllDevicesAsync(new[] { clientAcc.NotificationsId }, NotificationType.OffchainRequest, "Wallet");
            }
        }

        public async Task CreateOffchainRequestAndNotify(string transactionId, string clientId, string assetId, decimal amount,
            string orderId, OffchainTransferType type)
        {
            await CreateOffchainRequest(transactionId, clientId, assetId, amount, orderId, type);
            await NotifyUser(clientId);
        }

        public Task CreateOffchainRequestAndLock(string transactionId, string clientId, string assetId, decimal amount, string orderId,
            OffchainTransferType type)
        {
            return CreateAndAggregate(transactionId, clientId, assetId, amount, orderId, type, DateTime.UtcNow);
        }

        public async Task CreateOffchainRequestAndUnlock(string transactionId, string clientId, string assetId, decimal amount,
            string orderId, OffchainTransferType type)
        {
            await CreateAndAggregate(transactionId, clientId, assetId, amount, orderId, type, null);
            await NotifyUser(clientId);
        }

        private async Task CreateAndAggregate(string transactionId, string clientId, string assetId, decimal amount,
            string orderId, OffchainTransferType type, DateTime? lockTime)
        {
            var newTransfer = await _offchainTransferRepository.CreateTransfer(transactionId, clientId, assetId, amount, type, null, orderId);

            var request = await _offchainRequestRepository.CreateRequestAndLock(transactionId, clientId, assetId, RequestType.RequestTransfer, type, lockTime);

            await _pendingActionsClient.SetPendingAsync(clientId, PendingActionType.OffchainRequest, true);

            var oldTransferId = request.TransferId;

            //aggregate transfers
            if (oldTransferId != newTransfer.Id)
            {
                await _offchainTransferRepository.AddChildTransfer(oldTransferId, newTransfer);
                await _offchainTransferRepository.SetTransferIsChild(newTransfer.Id, oldTransferId);
            }
        }

        public async Task CreateHubCashoutRequests(string clientId, decimal bitcoinAmount = 0, decimal lkkAmount = 0)
        {
            if (bitcoinAmount > 0)
                await CreateOffchainRequest(Guid.NewGuid().ToString(), clientId, LykkeConstants.BitcoinAssetId, bitcoinAmount, null, OffchainTransferType.HubCashout);

            if (lkkAmount > 0)
                await CreateOffchainRequest(Guid.NewGuid().ToString(), clientId, LykkeConstants.LykkeAssetId, lkkAmount, null, OffchainTransferType.HubCashout);

            await NotifyUser(clientId);
        }
    }
}