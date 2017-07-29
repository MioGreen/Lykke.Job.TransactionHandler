using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Services.MarginTrading;
using Lykke.Job.TransactionHandler.Services.Generated.MarginApi;
using Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models;

namespace Lykke.Job.TransactionHandler.Services.MarginTrading
{
    public class MarginDataServiceSettings
    {
        public Uri BaseUri { get; set; }
        public string ApiKey { get; set; }
    }

    public class MarginDataService : IMarginDataService
    {
        private readonly MarginDataServiceSettings _settings;

        public MarginDataService(MarginDataServiceSettings settings)
        {
            _settings = settings;
        }

        private MarginTradingApi Api => new MarginTradingApi(_settings.BaseUri);

        public async Task<IEnumerable<MarginAssetsInfoRecord>> GetAssetsInfoAsync()
        {
            var result = await Api.ApiBackofficeAssetsInfoGetAsync(_settings.ApiKey);

            return result.ConvertToDto();
        }

        public async Task<IEnumerable<OrderRecord>> GetPositionsByVolumeAsync(double volume)
        {
            var result = await Api.ApiBackofficePositionsByVolumeGetAsync(_settings.ApiKey, volume);

            return result.ConvertToDto();
        }

        public async Task<IEnumerable<OrderRecord>> GetPendingOrdersByVolumeAsync(double volume)
        {
            var result = await Api.ApiBackofficePendingOrdersByVolumeGetAsync(_settings.ApiKey, volume);

            return result.ConvertToDto();
        }

        public async Task<string> InitAccounts(string clientId, string tradingConditions)
        {
            var request = new InitAccountsRequest { ClientId = clientId, TradingConditionsId = tradingConditions };
            var result = await Api.ApiBackofficeMarginTradingAccountsInitPostWithHttpMessagesAsync(_settings.ApiKey,
                request);

            return result.Body.Message;
        }

        #region Trading conditions

        public async Task<IEnumerable<TradingConditionRecord>> GetTradingConditionsAsync()
        {
            var result = await Api.ApiBackofficeTradingConditionsGetallGetAsync(_settings.ApiKey);

            return result.ConvertToDto();
        }

        public async Task<TradingConditionRecord> GetTradingConditionAsync(string id)
        {
            var result = await Api.ApiBackofficeTradingConditionsGetByIdGetAsync(id, _settings.ApiKey);

            return result.ConvertToDto();
        }

        public async Task AddOrEditTradingConditionAsync(TradingConditionRecord model)
        {
            await Api.ApiBackofficeTradingConditionsAddPostAsync(_settings.ApiKey, model.ConvertToDomain());
        }

        #endregion

        #region Account groups

        public async Task<IEnumerable<AccountGroupRecord>> GetAccountGroupsAsync()
        {
            var result = await Api.ApiBackofficeAccountGroupsGetallGetAsync(_settings.ApiKey);

            return result.ConvertToDto();
        }

        public async Task<AccountGroupRecord> GetAccountGroupAsync(string tradingConditionId, string id)
        {
            var result = await Api.ApiBackofficeAccountGroupsGetByTradingConditionIdByIdGetAsync(tradingConditionId, id, _settings.ApiKey);

            return result.ConvertToDto();
        }

        public async Task AddOrEditAccountGroupAsync(AccountGroupRecord model)
        {
            await Api.ApiBackofficeAccountGroupsAddPostAsync(_settings.ApiKey, model.ConvertToDomain());
        }

        #endregion

        #region Account assets

        public async Task<IEnumerable<AccountAssetRecord>> GetAccountAssetsAsync(string tradingConditionId, string baseAssetId)
        {
            var result = await Api.ApiBackofficeAccountAssetsGetallByTradingConditionIdByAccountAssetIdGetAsync(tradingConditionId, baseAssetId, _settings.ApiKey);

            return result.ConvertToDto();
        }

        public async Task<AccountAssetRecord> GetAccountAssetAsync(string tradingConditionId, string baseAssetId, string instrument)
        {
            var result = await Api.ApiBackofficeAccountAssetsGetByTradingConditionIdByBaseAssetIdByInstrumetGetAsync(tradingConditionId, baseAssetId, instrument, _settings.ApiKey);

            return result.ConvertToDto();
        }

        public async Task AssignInstrumentsAsync(string tradingConditionId, string baseAssetId, string[] instruments)
        {
            await Api.ApiBackofficeAccountAssetsAssignInstrumentsPostAsync(_settings.ApiKey, new AssignInstrumentsModel
            {
                TradingConditionId = tradingConditionId,
                BaseAssetId = baseAssetId,
                Instruments = instruments
            });
        }

        public async Task AddOrEditAccountAssetAsync(AccountAssetRecord model)
        {
            await Api.ApiBackofficeAccountAssetsAddPostAsync(_settings.ApiKey, model.ConvertToDomain());
        }

        public async Task<IEnumerable<InstrumentRecord>> GetAllInstrumentsAsync()
        {
            var result = await Api.ApiBackofficeInstrumentsGetallGetAsync(_settings.ApiKey);

            return result.ConvertToDto();
        }

        #endregion


        #region Accounts

        public async Task<IEnumerable<AccountRecord>> GetAccountsAsync(string clientId)
        {
            var result = await Api.ApiBackofficeMarginTradingAccountsGetallByClientIdGetAsync(clientId, _settings.ApiKey);
            return result.ConvertToDto();
        }

        public async Task DeleteAccountAsync(string clientId, string accountId)
        {
            await Api.ApiBackofficeMarginTradingAccountsDeleteByClientIdByAccountIdPostAsync(clientId, accountId, _settings.ApiKey);
        }

        public async Task AddAccountAsync(AccountRecord model)
        {
            await Api.ApiBackofficeMarginTradingAccountsAddPostAsync(_settings.ApiKey, model.ConvertToDomain());
        }

        public async Task<bool> DepositToAccount(string clientId, string accountId, double amount, MarginPaymentType paymentType)
        {
            var request = new AccountDepositWithdrawRequest
            {
                AccountId = accountId,
                ClientId = clientId,
                Amount = amount,
                PaymentType = paymentType.ConvertToDto()
            };
            var result = await Api.ApiBackofficeMarginTradingAccountsDepositPostWithHttpMessagesAsync(_settings.ApiKey,
                request);

            return result.Body == true;
        }

        public async Task<bool> WithdrawFromAccount(string clientId, string accountId, double amount, MarginPaymentType paymentType)
        {
            var request = new AccountDepositWithdrawRequest
            {
                AccountId = accountId,
                ClientId = clientId,
                Amount = amount,
                PaymentType = paymentType.ConvertToDto()
            };
            var result = await Api.ApiBackofficeMarginTradingAccountsWithdrawPostWithHttpMessagesAsync(_settings.ApiKey,
                request);

            return result.Body == true;
        }

        public async Task<bool> ResetAccount(string clientId, string accountId)
        {
            var request = new AccounResetRequest
            {
                AccountId = accountId,
                ClientId = clientId
            };
            var result = await Api.ApiBackofficeMarginTradingAccountsResetPostWithHttpMessagesAsync(_settings.ApiKey,
                request);

            return result.Body == true;
        }

        #endregion
    }

    internal static class DtoConvertor
    {
        public static IEnumerable<MarginAssetsInfoRecord> ConvertToDto(this IEnumerable<SummaryAssetInfo> assetInfos)
        {
            return assetInfos.Select(x => new MarginAssetsInfoRecord
            {
                AssetPairId = x.AssetPairId,
                PnL = x.PnL.GetValueOrDefault(),
                VolumeLong = x.VolumeLong.GetValueOrDefault(),
                VolumeShort = x.VolumeShort.GetValueOrDefault()
            });
        }

        public static IEnumerable<OrderRecord> ConvertToDto(this IEnumerable<OrderContract> orders)
        {
            return orders.Select(x => new OrderRecord
            {
                Instrument = x.Instrument,
                OpenDate = x.OpenDate.GetValueOrDefault(),
                OpenPrice = x.OpenPrice.GetValueOrDefault(),
                PnL = x.PnL.GetValueOrDefault(),
                Volume = x.Volume.GetValueOrDefault(),
                ExpectedOpenPrice = x.ExpectedOpenPrice.GetValueOrDefault(),
                CreateDate = x.CreateDate.GetValueOrDefault()
            });
        }

        public static MarginTradingCondition ConvertToDomain(this TradingConditionRecord condition)
        {
            return new MarginTradingCondition
            {
                Id = condition.Id,
                Name = condition.Name,
                IsDefault = condition.IsDefault
            };
        }

        public static TradingConditionRecord ConvertToDto(this MarginTradingCondition condition)
        {
            return new TradingConditionRecord
            {
                Id = condition.Id,
                Name = condition.Name,
                IsDefault = condition.IsDefault.GetValueOrDefault()
            };
        }

        public static IEnumerable<TradingConditionRecord> ConvertToDto(this IEnumerable<MarginTradingCondition> conditions)
        {
            return conditions.Select(ConvertToDto);
        }

        public static MarginTradingAccountGroup ConvertToDomain(this AccountGroupRecord group)
        {
            return new MarginTradingAccountGroup
            {
                BaseAssetId = group.BaseAssetId,
                MarginCall = group.MarginCall,
                StopOut = group.StopOut,
                TradingConditionId = group.TradingConditionId,
                DepositTransferLimit = group.DepositTransferLimit
            };
        }

        public static AccountGroupRecord ConvertToDto(this MarginTradingAccountGroup group)
        {
            return new AccountGroupRecord
            {
                BaseAssetId = group.BaseAssetId,
                MarginCall = group.MarginCall.GetValueOrDefault(),
                StopOut = group.StopOut.GetValueOrDefault(),
                TradingConditionId = group.TradingConditionId,
                DepositTransferLimit = group.DepositTransferLimit.GetValueOrDefault()
            };
        }

        public static IEnumerable<AccountGroupRecord> ConvertToDto(this IEnumerable<MarginTradingAccountGroup> groups)
        {
            return groups.Select(ConvertToDto);
        }


        public static AccountAssetRecord ConvertToDto(this MarginTradingAccountAsset asset)
        {
            return new AccountAssetRecord
            {
                TradingConditionId = asset.TradingConditionId,
                BaseAssetId = asset.BaseAssetId,
                Instrument = asset.Instrument,
                LeverageInit = asset.LeverageInit.GetValueOrDefault(),
                LeverageMaintenance = asset.LeverageMaintenance.GetValueOrDefault(),
                SwapLong = asset.SwapLong.GetValueOrDefault(),
                SwapShort = asset.SwapShort.GetValueOrDefault(),
                SwapLongPct = asset.SwapLongPct.GetValueOrDefault(),
                SwapShortPct = asset.SwapShortPct.GetValueOrDefault(),
                CommissionLong = asset.CommissionLong.GetValueOrDefault(),
                CommissionShort = asset.CommissionShort.GetValueOrDefault(),
                CommissionLot = asset.CommissionLot.GetValueOrDefault(),
                DeltaBid = asset.DeltaBid.GetValueOrDefault(),
                DeltaAsk = asset.DeltaAsk.GetValueOrDefault(),
                PositionLimit = asset.PositionLimit.GetValueOrDefault(),
                DealLimit = asset.DealLimit.GetValueOrDefault()
            };
        }

        public static IEnumerable<AccountAssetRecord> ConvertToDto(this IEnumerable<MarginTradingAccountAsset> assets)
        {
            return assets.Select(ConvertToDto);
        }

        public static MarginTradingAccountAsset ConvertToDomain(this AccountAssetRecord asset)
        {
            return new MarginTradingAccountAsset
            {
                TradingConditionId = asset.TradingConditionId,
                BaseAssetId = asset.BaseAssetId,
                Instrument = asset.Instrument,
                LeverageInit = asset.LeverageInit,
                LeverageMaintenance = asset.LeverageMaintenance,
                SwapLong = asset.SwapLong,
                SwapShort = asset.SwapShort,
                SwapLongPct = asset.SwapLongPct,
                SwapShortPct = asset.SwapShortPct,
                CommissionLong = asset.CommissionLong,
                CommissionShort = asset.CommissionShort,
                CommissionLot = asset.CommissionLot,
                DeltaBid = asset.DeltaBid,
                DeltaAsk = asset.DeltaAsk,
                PositionLimit = asset.PositionLimit,
                DealLimit = asset.DealLimit
            };
        }

        public static InstrumentRecord ConvertToDto(this MarginTradingAsset asset)
        {
            return new InstrumentRecord
            {

                Id = asset.Id,
                Name = asset.Name,
                BaseAssetId = asset.BaseAssetId,
                QuoteAssetId = asset.QuoteAssetId,
                Accuracy = asset.Accuracy.GetValueOrDefault()
            };
        }

        public static IEnumerable<InstrumentRecord> ConvertToDto(this IEnumerable<MarginTradingAsset> assets)
        {
            return assets.Select(ConvertToDto);
        }

        public static AccountRecord ConvertToDto(this MarginTradingAccount account)
        {
            return new AccountRecord
            {

                Id = account.Id,
                ClientId = account.ClientId,
                TradingConditionId = account.TradingConditionId,
                BaseAssetId = account.BaseAssetId,
                Balance = account.Balance.GetValueOrDefault(),
                WithdrawTransferLimit = account.WithdrawTransferLimit.GetValueOrDefault()
            };
        }

        public static IEnumerable<AccountRecord> ConvertToDto(this IEnumerable<MarginTradingAccount> accounts)
        {
            return accounts.Select(ConvertToDto);
        }

        public static MarginTradingAccount ConvertToDomain(this AccountRecord account)
        {
            return new MarginTradingAccount
            {
                Id = account.Id,
                ClientId = account.ClientId,
                TradingConditionId = account.TradingConditionId,
                BaseAssetId = account.BaseAssetId,
                Balance = account.Balance,
                WithdrawTransferLimit = account.WithdrawTransferLimit
            };
        }

        public static PaymentType ConvertToDto(this MarginPaymentType paymentType)
        {
            switch (paymentType)
            {
                case MarginPaymentType.Swift:
                    return PaymentType.Swift;

                case MarginPaymentType.Transfer:
                    return PaymentType.Transfer;
            }

            throw new ArgumentOutOfRangeException(nameof(paymentType), paymentType, string.Empty);
        }
    }

}