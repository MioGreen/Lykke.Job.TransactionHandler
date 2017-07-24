using System;
using System.Linq;
using System.Threading.Tasks;
using LkeServices.Generated.BitcoinCoreApi;
using LkeServices.Generated.BitcoinCoreApi.Models;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin.BitCoinApi;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin.BitCoinApi.Models;

namespace Lykke.Job.TransactionHandler.Services.BitCoin.BitCoinApiClient
{
    public class BitcoinApiClient : IBitcoinApiClient
    {
        private readonly BitcoinApi _apiClient;

        public BitcoinApiClient(AppSettings.BitcoinCoreSettings settings)
        {
            _apiClient = new BitcoinApi(new Uri(settings.BitcoinCoreApiUrl));
            _apiClient.SetRetryPolicy(null);
        }

        public async Task<OnchainResponse> IssueAsync(IssueData data)
        {
            var request = new IssueRequest(data.TransactionId, data.Address, data.AssetId, (decimal)data.Amount);

            var response = await _apiClient.ApiEnqueueTransactionIssuePostAsync(request);

            return PrepareOnchainResult(response);
        }

        public async Task<OnchainResponse> TransferAsync(TransferData data)
        {
            var request = new TransferRequest(data.TransactionId, data.SourceAddress, data.DestinationAddress,
                (decimal)data.Amount, data.AssetId);

            var response = await _apiClient.ApiEnqueueTransactionTransferPostAsync(request);

            return PrepareOnchainResult(response);
        }

        public async Task<OnchainResponse> TransferAllAsync(TransferAllData data)
        {
            var request = new TransferAllRequest(data.TransactionId, data.SourceAddress, data.DestinationAddress);

            var response = await _apiClient.ApiEnqueueTransactionTransferallPostAsync(request);

            return PrepareOnchainResult(response);
        }

        public async Task<OnchainResponse> DestroyAsync(DestroyData data)
        {
            var request = new DestroyRequest(data.TransactionId, data.Address, data.AssetId, (decimal)data.Amount);

            var response = await _apiClient.ApiEnqueueTransactionDestroyPostAsync(request);

            return PrepareOnchainResult(response);
        }

        public async Task<OnchainResponse> SwapAsyncTransaction(SwapData data)
        {
            var request = new SwapRequest(data.TransactionId, data.Multisig1, (decimal)data.Amount1, data.AssetId1,
                data.Multisig2, (decimal)data.Amount2, data.AssetId2);

            var response = await _apiClient.ApiEnqueueTransactionSwapPostAsync(request);

            return PrepareOnchainResult(response);
        }

        public async Task<OnchainResponse> RetryAsync(RetryData data)
        {
            var request = new RetryFailedRequest(data.TransactionId);

            var response = await _apiClient.ApiEnqueueTransactionRetryPostAsync(request);

            if (response != null)
                return new OnchainResponse
                {
                    Error = new ErrorResponse { Code = response.Error.Code, Message = response.Error.Message }
                };

            return new OnchainResponse();
        }

        public async Task<OffchainResponse> OffchainTransferAsync(OffchainTransferData data)
        {
            var request = new TransferModel(data.ClientPubKey, data.Amount, data.AssetId, data.ClientPrevPrivateKey, data.Required, !string.IsNullOrWhiteSpace(data.ExternalTransferId) ? Guid.Parse(data.ExternalTransferId) : (Guid?)null);

            var response = await _apiClient.ApiOffchainTransferPostAsync(request);

            return PrepareOffchainResult(response);
        }

        public async Task<OffchainResponse> CreateChannelAsync(CreateChannelData data)
        {
            var request = new CreateChannelModel(data.ClientPubKey, data.HotWalletAddress, data.HubAmount, data.AssetId, data.Required, !string.IsNullOrWhiteSpace(data.ExternalTransferId) ? Guid.Parse(data.ExternalTransferId) : (Guid?)null);

            var response = await _apiClient.ApiOffchainCreatechannelPostAsync(request);

            return PrepareOffchainResult(response);
        }

        public async Task<OffchainResponse> CreateHubCommitment(CreateHubComitmentData data)
        {
            var request = new CreateHubCommitmentModel(data.ClientPubKey, data.Amount, data.AssetId, data.SignedByClientChannel);

            var response = await _apiClient.ApiOffchainCreatehubcommitmentPostAsync(request);

            return PrepareOffchainResult(response);
        }

        public async Task<OffchainResponse> Finalize(FinalizeData data)
        {
            var request = new FinalizeChannelModel(data.ClientPubKey, data.HotWalletAddress, data.AssetId, data.ClientRevokePubKey, data.SignedByClientHubCommitment, !string.IsNullOrWhiteSpace(data.ExternalTransferId) ? Guid.Parse(data.ExternalTransferId) : (Guid?)null, !string.IsNullOrWhiteSpace(data.OffchainTransferId) ? Guid.Parse(data.OffchainTransferId) : (Guid?)null);

            var response = await _apiClient.ApiOffchainFinalizePostAsync(request);

            return PrepareFinalizeOffchainResult(response);
        }

        public async Task<OffchainClosingResponse> Cashout(CashoutData data)
        {
            var request = new CashoutModel(data.ClientPubKey, data.CashoutAddress, data.HotWalletAddress, data.AssetId, data.Amount);

            var response = await _apiClient.ApiOffchainCashoutPostAsync(request);

            return PrepareOffchainClosingResult(response);
        }

        public async Task<OffchainBaseResponse> CloseChannel(CloseChannelData data)
        {
            var request = new BroadcastClosingChannelModel(data.ClientPubKey, data.AssetId, data.SignedClosingTransaction, !string.IsNullOrWhiteSpace(data.OffchainTransferId) ? Guid.Parse(data.OffchainTransferId) : (Guid?)null);

            var response = await _apiClient.ApiOffchainBroadcastclosingPostAsync(request);

            return PrepareOffchainTransactionHashResult(response);
        }

        public async Task<OffchainClosingResponse> HubCashout(HubCashoutData data)
        {
            var request = new CreateCashoutFromHubModel(data.ClientPubKey, data.Hotwallet, data.AssetId);

            var response = await _apiClient.ApiOffchainCashouthubPostAsync(request);

            return PrepareOffchainClosingResult(response);
        }

        public async Task<OffchainAssetBalancesResponse> ChannelsInfo(string asset)
        {
            var response = await _apiClient.ApiOffchainAssetBalancesGetAsync(asset);

            var error = response as ApiException;

            if (error != null)
            {
                return new OffchainAssetBalancesResponse
                {
                    Error = new ErrorResponse { Code = error.Error.Code, Message = error.Error.Message }
                };
            }

            var balances = response as AssetBalanceInfoResponse;

            if (balances != null)
                return new OffchainAssetBalancesResponse
                {
                    Balances = balances.Balances.Select(x => new OffchainChannelBalance
                    {
                        Multisig = x.Multisig,
                        ClientAmount = x.ClientAmount ?? 0,
                        HubAmount = x.HubAmount ?? 0
                    })
                };

            throw new ArgumentException("Unkown response object");
        }

        public async Task<OffchainBalancesResponse> Balances(string multisig)
        {
            var response = await _apiClient.ApiOffchainBalancesGetAsync(multisig);

            var error = response as ApiException;

            if (error != null)
            {
                return new OffchainBalancesResponse
                {
                    Error = new ErrorResponse { Code = error.Error.Code, Message = error.Error.Message }
                };
            }

            var balances = response as OffchainBalanceResponse;

            if (balances != null)
                return new OffchainBalancesResponse
                {
                    Balances = balances.Channels.ToDictionary(x => x.Key, y => new OffchainChannelBalance
                    {
                        Hash = y.Value.TransactionHash,
                        ClientAmount = y.Value.ClientAmount ?? 0,
                        HubAmount = y.Value.HubAmount ?? 0,
                        Actual = y.Value.Actual.GetValueOrDefault()
                    })
                };

            throw new ArgumentException("Unkown response object");
        }

        private OnchainResponse PrepareOnchainResult(object response)
        {
            var error = response as ApiException;
            var transaction = response as TransactionIdResponse;

            if (error != null)
            {
                return new OnchainResponse
                {
                    Error = new ErrorResponse { Code = error.Error.Code, Message = error.Error.Message }
                };
            }

            if (transaction != null)
            {
                return new OnchainResponse
                {
                    Transaction =
                        new TransactionRepsonse
                        {
                            TransactionId = transaction.TransactionId
                        }
                };
            }

            throw new ArgumentException("Unkown response object");
        }

        private OffchainClosingResponse PrepareOffchainClosingResult(object response)
        {
            var error = response as ApiException;

            if (error != null)
            {
                return new OffchainClosingResponse
                {
                    Error = new ErrorResponse { Code = error.Error.Code, Message = error.Error.Message }
                };
            }

            var cashout = response as CashoutOffchainApiResponse;
            if (cashout != null)
            {
                return new OffchainClosingResponse
                {
                    Transaction = cashout.Transaction,
                    TransferId = cashout.TransferId,
                    ChannelClosing = cashout.ChannelClosed ?? false
                };
            }

            throw new ArgumentException("Unkown response object");
        }

        private OffchainResponse PrepareOffchainResult(object response)
        {
            var error = response as ApiException;

            if (error != null)
            {
                return new OffchainResponse
                {
                    Error = new ErrorResponse { Code = error.Error.Code, Message = error.Error.Message }
                };
            }

            var transaction = response as OffchainApiResponse;
            if (transaction != null)
            {
                return new OffchainResponse
                {
                    Transaction = transaction.Transaction,
                    TransferId = transaction.TransferId
                };
            }

            throw new ArgumentException("Unkown response object");
        }

        private OffchainResponse PrepareFinalizeOffchainResult(object response)
        {
            var error = response as ApiException;

            if (error != null)
            {
                return new OffchainResponse
                {
                    Error = new ErrorResponse { Code = error.Error.Code, Message = error.Error.Message }
                };
            }

            var transaction = response as FinalizeOffchainApiResponse;
            if (transaction != null)
            {
                return new OffchainResponse
                {
                    Transaction = transaction.Transaction,
                    TransferId = transaction.TransferId,
                    TxHash = transaction.Hash
                };
            }

            throw new ArgumentException("Unkown response object");
        }

        private OffchainBaseResponse PrepareOffchainTransactionHashResult(object response)
        {
            var error = response as ApiException;

            if (error != null)
            {
                return new OffchainBaseResponse
                {
                    Error = new ErrorResponse { Code = error.Error.Code, Message = error.Error.Message }
                };
            }

            var transaction = response as TransactionHashResponse;
            if (transaction != null)
            {
                return new OffchainBaseResponse
                {
                    TxHash = transaction.TransactionHash
                };
            }

            throw new ArgumentException("Unkown response object");
        }
    }

}