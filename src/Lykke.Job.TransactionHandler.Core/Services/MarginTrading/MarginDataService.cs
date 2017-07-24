using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Services.MarginTrading
{
    public interface IMarginDataService
    {
        Task<IEnumerable<MarginAssetsInfoRecord>> GetAssetsInfoAsync();
        Task<IEnumerable<OrderRecord>> GetPositionsByVolumeAsync(double volume);
        Task<IEnumerable<OrderRecord>> GetPendingOrdersByVolumeAsync(double volume);

        Task<IEnumerable<TradingConditionRecord>> GetTradingConditionsAsync();
        Task<TradingConditionRecord> GetTradingConditionAsync(string id);
        Task AddOrEditTradingConditionAsync(TradingConditionRecord model);

        Task<IEnumerable<AccountGroupRecord>> GetAccountGroupsAsync();
        Task<AccountGroupRecord> GetAccountGroupAsync(string tradingConditionId, string id);
        Task AddOrEditAccountGroupAsync(AccountGroupRecord model);

        Task<IEnumerable<AccountAssetRecord>> GetAccountAssetsAsync(string tradingConditionId, string baseAssetId);
        Task<AccountAssetRecord> GetAccountAssetAsync(string tradingConditionId, string baseAssetId, string instrument);
        Task AssignInstrumentsAsync(string tradingConditionId, string baseAssetId, string[] instruments);
        Task AddOrEditAccountAssetAsync(AccountAssetRecord model);

        Task<IEnumerable<InstrumentRecord>> GetAllInstrumentsAsync();

        Task<IEnumerable<AccountRecord>> GetAccountsAsync(string clientId);
        Task DeleteAccountAsync(string clientId, string accountId);
        Task AddAccountAsync(AccountRecord model);
        Task<string> InitAccounts(string clientId, string tradingConditions);

        Task<bool> DepositToAccount(string clientId, string accountId, double amount, MarginPaymentType paymentType);
        Task<bool> WithdrawFromAccount(string clientId, string accountId, double amount, MarginPaymentType paymentType);
        Task<bool> ResetAccount(string clientId, string accountId);
    }

    public class MarginAssetsInfoRecord
    {
        public string AssetPairId { get; set; }
        public double VolumeLong { get; set; }
        public double VolumeShort { get; set; }
        public double PnL { get; set; }
    }

    public class OrderRecord
    {
        public string Instrument { get; set; }
        public DateTime OpenDate { get; set; }
        public double OpenPrice { get; set; }
        public double PnL { get; set; }
        public double Volume { get; set; }
        public double ExpectedOpenPrice { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class TradingConditionRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }

    public class AccountGroupRecord
    {
        public string TradingConditionId { get; set; }
        public string BaseAssetId { get; set; }
        public double MarginCall { get; set; }
        public double StopOut { get; set; }
        public double DepositTransferLimit { get; set; }
    }

    public class AccountAssetRecord
    {
        public string TradingConditionId { get; set; }
        public string BaseAssetId { get; set; }
        public string Instrument { get; set; }
        public int LeverageInit { get; set; }
        public int LeverageMaintenance { get; set; }
        public double SwapLong { get; set; }
        public double SwapShort { get; set; }
        public double SwapLongPct { get; set; }
        public double SwapShortPct { get; set; }
        public double CommissionLong { get; set; }
        public double CommissionShort { get; set; }
        public double CommissionLot { get; set; }
        public double DeltaBid { get; set; }
        public double DeltaAsk { get; set; }
        public double DealLimit { get; set; }
        public double PositionLimit { get; set; }
    }

    public class InstrumentRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BaseAssetId { get; set; }
        public string QuoteAssetId { get; set; }
        public int Accuracy { get; set; }
    }

    public class AccountRecord
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string TradingConditionId { get; set; }
        public string BaseAssetId { get; set; }
        public double Balance { get; set; }
        public double WithdrawTransferLimit { get; set; }
    }

    public enum MarginPaymentType
    {
        Transfer,
        Swift
    }
}