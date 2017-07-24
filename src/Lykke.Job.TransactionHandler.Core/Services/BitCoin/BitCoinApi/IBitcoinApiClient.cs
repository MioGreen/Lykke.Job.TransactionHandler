using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin.BitCoinApi.Models;

namespace Lykke.Job.TransactionHandler.Core.Services.BitCoin.BitCoinApi
{
    public interface IBitcoinApiClient
    {
        //onchain
        Task<OnchainResponse> IssueAsync(IssueData data);
        Task<OnchainResponse> TransferAsync(TransferData data);
        Task<OnchainResponse> TransferAllAsync(TransferAllData data);
        Task<OnchainResponse> DestroyAsync(DestroyData data);
        Task<OnchainResponse> SwapAsyncTransaction(SwapData data);
        Task<OnchainResponse> RetryAsync(RetryData data);

        //offchain
        Task<OffchainResponse> OffchainTransferAsync(OffchainTransferData data);
        Task<OffchainResponse> CreateChannelAsync(CreateChannelData data);
        Task<OffchainResponse> CreateHubCommitment(CreateHubComitmentData data);
        Task<OffchainResponse> Finalize(FinalizeData data);
        Task<OffchainClosingResponse> Cashout(CashoutData data);
        Task<OffchainBaseResponse> CloseChannel(CloseChannelData data);
        Task<OffchainClosingResponse> HubCashout(HubCashoutData data);
        Task<OffchainBalancesResponse> Balances(string multisig);
        Task<OffchainAssetBalancesResponse> ChannelsInfo(string asset);
    }
}