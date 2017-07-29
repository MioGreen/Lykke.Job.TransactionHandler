// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Services.Generated.Quanta
{
    /// <summary>
    /// Extension methods for QuantaApiClient.
    /// </summary>
    public static partial class QuantaApiClientExtensions
    {
            /// <summary>
            /// Gets new ehtereum user contract and saves it for monitoring
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object ApiClientRegisterGet(this IQuantaApiClient operations)
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IQuantaApiClient)s).ApiClientRegisterGetAsync(), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets new ehtereum user contract and saves it for monitoring
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<object> ApiClientRegisterGetAsync(this IQuantaApiClient operations, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientRegisterGetWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='address'>
            /// </param>
            public static object ApiClientIsQuantaUserGet(this IQuantaApiClient operations, string address = default(string))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IQuantaApiClient)s).ApiClientIsQuantaUserGetAsync(address), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='address'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<object> ApiClientIsQuantaUserGetAsync(this IQuantaApiClient operations, string address = default(string), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientIsQuantaUserGetWithHttpMessagesAsync(address, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Check API is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static void ApiIsAliveGet(this IQuantaApiClient operations)
            {
                System.Threading.Tasks.Task.Factory.StartNew(s => ((IQuantaApiClient)s).ApiIsAliveGetAsync(), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None,  System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Check API is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task ApiIsAliveGetAsync(this IQuantaApiClient operations, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                await operations.ApiIsAliveGetWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false);
            }

            /// <summary>
            /// Check Ethereum RPC is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static void ApiIsAliveRpcGet(this IQuantaApiClient operations)
            {
                System.Threading.Tasks.Task.Factory.StartNew(s => ((IQuantaApiClient)s).ApiIsAliveRpcGetAsync(), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None,  System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Check Ethereum RPC is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task ApiIsAliveRpcGetAsync(this IQuantaApiClient operations, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                await operations.ApiIsAliveRpcGetWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false);
            }

    }
}
