using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Offchain
{
    public enum OffchainTransferType
    {
        None = 0,
        FromClient = 1,
        FromHub = 2,
        CashinFromClient = 3,
        ClientCashout = 4,
        FullCashout = 5, // not used
        CashinToClient = 6,
        OffchainCashout = 7,
        HubCashout = 8,
        DirectTransferFromClient = 9,
        FromClientLimit = 1,
    }

    public interface IOffchainTransfer
    {
        string Id { get; }
        string ClientId { get; }
        string AssetId { get; }
        decimal Amount { get; }
        bool Completed { get; }
        string OrderId { get; }
        DateTime CreatedDt { get; }
        string ExternalTransferId { get; }
        OffchainTransferType Type { get; }
        bool ChannelClosing { get; }
        bool Onchain { get; }
    }

    public interface IOffchainTransferRepository
    {
        Task<IOffchainTransfer> CreateTransfer(string transactionId, string clientId, string assetId, decimal amount, OffchainTransferType type, string externalTransferId, string orderId, bool channelClosing = false);

        Task<IOffchainTransfer> GetTransfer(string id);

        Task<IEnumerable<IOffchainTransfer>> GetTransfersByOrder(string clientId, string orderId);

        Task CompleteTransfer(string transferId, bool? onchain = null);

        Task UpdateTransfer(string transferId, string toString, bool closing = false, bool? onchain = null);

        Task<IEnumerable<IOffchainTransfer>> GetTransfersByDate(OffchainTransferType type, DateTimeOffset from, DateTimeOffset to);
    }
}