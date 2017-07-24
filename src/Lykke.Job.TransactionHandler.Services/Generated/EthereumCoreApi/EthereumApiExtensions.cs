// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.EthereumCoreApi
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for EthereumApi.
    /// </summary>
    public static partial class EthereumApiExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiExchangeCashoutPost(this IEthereumApi operations, CashoutModel model = default(CashoutModel))
            {
                return operations.ApiExchangeCashoutPostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiExchangeCashoutPostAsync(this IEthereumApi operations, CashoutModel model = default(CashoutModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExchangeCashoutPostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='guid'>
            /// </param>
            public static CheckIdResponse ApiExchangeCheckIdByGuidGet(this IEthereumApi operations, System.Guid guid)
            {
                return operations.ApiExchangeCheckIdByGuidGetAsync(guid).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='guid'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<CheckIdResponse> ApiExchangeCheckIdByGuidGetAsync(this IEthereumApi operations, System.Guid guid, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExchangeCheckIdByGuidGetWithHttpMessagesAsync(guid, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiExchangeTransferPost(this IEthereumApi operations, TransferModel model = default(TransferModel))
            {
                return operations.ApiExchangeTransferPostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiExchangeTransferPostAsync(this IEthereumApi operations, TransferModel model = default(TransferModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExchangeTransferPostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiExchangeCheckSignPost(this IEthereumApi operations, CheckSignModel model = default(CheckSignModel))
            {
                return operations.ApiExchangeCheckSignPostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiExchangeCheckSignPostAsync(this IEthereumApi operations, CheckSignModel model = default(CheckSignModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExchangeCheckSignPostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiExchangeTransferWithChangePost(this IEthereumApi operations, TransferWithChangeModel model = default(TransferWithChangeModel))
            {
                return operations.ApiExchangeTransferWithChangePostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiExchangeTransferWithChangePostAsync(this IEthereumApi operations, TransferWithChangeModel model = default(TransferWithChangeModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExchangeTransferWithChangePostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiExchangeCheckPendingTransactionPost(this IEthereumApi operations, CheckPendingModel model = default(CheckPendingModel))
            {
                return operations.ApiExchangeCheckPendingTransactionPostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiExchangeCheckPendingTransactionPostAsync(this IEthereumApi operations, CheckPendingModel model = default(CheckPendingModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExchangeCheckPendingTransactionPostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiExchangeEstimateCashoutGasPost(this IEthereumApi operations, TransferModel model = default(TransferModel))
            {
                return operations.ApiExchangeEstimateCashoutGasPostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiExchangeEstimateCashoutGasPostAsync(this IEthereumApi operations, TransferModel model = default(TransferModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExchangeEstimateCashoutGasPostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static ListResultExternalTokenModel ApiExternalTokenGet(this IEthereumApi operations)
            {
                return operations.ApiExternalTokenGetAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ListResultExternalTokenModel> ApiExternalTokenGetAsync(this IEthereumApi operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExternalTokenGetWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='externalTokenAddress'>
            /// </param>
            public static ExternalTokenModel ApiExternalTokenByExternalTokenAddressGet(this IEthereumApi operations, string externalTokenAddress)
            {
                return operations.ApiExternalTokenByExternalTokenAddressGetAsync(externalTokenAddress).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='externalTokenAddress'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ExternalTokenModel> ApiExternalTokenByExternalTokenAddressGetAsync(this IEthereumApi operations, string externalTokenAddress, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExternalTokenByExternalTokenAddressGetWithHttpMessagesAsync(externalTokenAddress, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static ExternalTokenModel ApiExternalTokenCreatePost(this IEthereumApi operations, CreateExternalTokenModel model = default(CreateExternalTokenModel))
            {
                return operations.ApiExternalTokenCreatePostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ExternalTokenModel> ApiExternalTokenCreatePostAsync(this IEthereumApi operations, CreateExternalTokenModel model = default(CreateExternalTokenModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExternalTokenCreatePostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static ExternalTokenModel ApiExternalTokenIssuePost(this IEthereumApi operations, IssueTokensModel model = default(IssueTokensModel))
            {
                return operations.ApiExternalTokenIssuePostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ExternalTokenModel> ApiExternalTokenIssuePostAsync(this IEthereumApi operations, IssueTokensModel model = default(IssueTokensModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExternalTokenIssuePostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='externalTokenAddress'>
            /// </param>
            /// <param name='ownerAddress'>
            /// </param>
            public static BalanceModel ApiExternalTokenBalanceByExternalTokenAddressByOwnerAddressGet(this IEthereumApi operations, string externalTokenAddress, string ownerAddress)
            {
                return operations.ApiExternalTokenBalanceByExternalTokenAddressByOwnerAddressGetAsync(externalTokenAddress, ownerAddress).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='externalTokenAddress'>
            /// </param>
            /// <param name='ownerAddress'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<BalanceModel> ApiExternalTokenBalanceByExternalTokenAddressByOwnerAddressGetAsync(this IEthereumApi operations, string externalTokenAddress, string ownerAddress, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiExternalTokenBalanceByExternalTokenAddressByOwnerAddressGetWithHttpMessagesAsync(externalTokenAddress, ownerAddress, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiHashCalculateAndGetIdPost(this IEthereumApi operations, BaseCoinRequestParametersModel model = default(BaseCoinRequestParametersModel))
            {
                return operations.ApiHashCalculateAndGetIdPostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiHashCalculateAndGetIdPostAsync(this IEthereumApi operations, BaseCoinRequestParametersModel model = default(BaseCoinRequestParametersModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiHashCalculateAndGetIdPostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiHashCalculatePost(this IEthereumApi operations, BaseCoinRequestModel model = default(BaseCoinRequestModel))
            {
                return operations.ApiHashCalculatePostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiHashCalculatePostAsync(this IEthereumApi operations, BaseCoinRequestModel model = default(BaseCoinRequestModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiHashCalculatePostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static void ApiSystemAmialiveGet(this IEthereumApi operations)
            {
                operations.ApiSystemAmialiveGetAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task ApiSystemAmialiveGetAsync(this IEthereumApi operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.ApiSystemAmialiveGetWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ApiTransitionCreatePost(this IEthereumApi operations, CreateTransitionContractModel model = default(CreateTransitionContractModel))
            {
                return operations.ApiTransitionCreatePostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiTransitionCreatePostAsync(this IEthereumApi operations, CreateTransitionContractModel model = default(CreateTransitionContractModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiTransitionCreatePostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userAddress'>
            /// </param>
            /// <param name='coinAdapterAddress'>
            /// </param>
            public static object ApiTransitionContractAddressByUserAddressByCoinAdapterAddressGet(this IEthereumApi operations, string userAddress, string coinAdapterAddress)
            {
                return operations.ApiTransitionContractAddressByUserAddressByCoinAdapterAddressGetAsync(userAddress, coinAdapterAddress).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userAddress'>
            /// </param>
            /// <param name='coinAdapterAddress'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiTransitionContractAddressByUserAddressByCoinAdapterAddressGetAsync(this IEthereumApi operations, string userAddress, string coinAdapterAddress, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiTransitionContractAddressByUserAddressByCoinAdapterAddressGetWithHttpMessagesAsync(userAddress, coinAdapterAddress, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static ListResultCoinResult ApiCoinAdapterGet(this IEthereumApi operations)
            {
                return operations.ApiCoinAdapterGetAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ListResultCoinResult> ApiCoinAdapterGetAsync(this IEthereumApi operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiCoinAdapterGetWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='adapterAddress'>
            /// </param>
            public static ExistsModel ApiCoinAdapterExistsByAdapterAddressGet(this IEthereumApi operations, string adapterAddress)
            {
                return operations.ApiCoinAdapterExistsByAdapterAddressGetAsync(adapterAddress).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='adapterAddress'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ExistsModel> ApiCoinAdapterExistsByAdapterAddressGetAsync(this IEthereumApi operations, string adapterAddress, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiCoinAdapterExistsByAdapterAddressGetWithHttpMessagesAsync(adapterAddress, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static CoinResult ApiCoinAdapterByIdGet(this IEthereumApi operations, string id)
            {
                return operations.ApiCoinAdapterByIdGetAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<CoinResult> ApiCoinAdapterByIdGetAsync(this IEthereumApi operations, string id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiCoinAdapterByIdGetWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='adapterAddress'>
            /// </param>
            public static CoinResult ApiCoinAdapterAddressByAdapterAddressGet(this IEthereumApi operations, string adapterAddress)
            {
                return operations.ApiCoinAdapterAddressByAdapterAddressGetAsync(adapterAddress).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='adapterAddress'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<CoinResult> ApiCoinAdapterAddressByAdapterAddressGetAsync(this IEthereumApi operations, string adapterAddress, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiCoinAdapterAddressByAdapterAddressGetWithHttpMessagesAsync(adapterAddress, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static RegisterResponse ApiCoinAdapterCreatePost(this IEthereumApi operations, CreateAssetModel model = default(CreateAssetModel))
            {
                return operations.ApiCoinAdapterCreatePostAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<RegisterResponse> ApiCoinAdapterCreatePostAsync(this IEthereumApi operations, CreateAssetModel model = default(CreateAssetModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiCoinAdapterCreatePostWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='coinAdapterAddress'>
            /// </param>
            /// <param name='userAddress'>
            /// </param>
            public static object ApiCoinAdapterBalanceByCoinAdapterAddressByUserAddressGet(this IEthereumApi operations, string coinAdapterAddress, string userAddress)
            {
                return operations.ApiCoinAdapterBalanceByCoinAdapterAddressByUserAddressGetAsync(coinAdapterAddress, userAddress).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='coinAdapterAddress'>
            /// </param>
            /// <param name='userAddress'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiCoinAdapterBalanceByCoinAdapterAddressByUserAddressGetAsync(this IEthereumApi operations, string coinAdapterAddress, string userAddress, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiCoinAdapterBalanceByCoinAdapterAddressByUserAddressGetWithHttpMessagesAsync(coinAdapterAddress, userAddress, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
