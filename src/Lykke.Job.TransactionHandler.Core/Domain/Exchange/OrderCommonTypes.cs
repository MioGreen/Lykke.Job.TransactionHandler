using System;

namespace Lykke.Job.TransactionHandler.Core.Domain.Exchange
{
    public enum OrderType
    {
        Buy, Sell
    }

    public enum OrderStatus
    {
        //Init status, limit order in order book
        InOrderBook
        //Partially matched
        , Processing
        //Fully matched
        , Matched
        //Not enough funds on account
        , NotEnoughFunds
        //Reserved volume greater than balance
        , ReservedVolumeGreaterThanBalance
        //No liquidity
        , NoLiquidity
        //Unknown asset
        , UnknownAsset
        //One of trades or whole order has volume/price*volume less then configured dust
        , Dust
        //Cancelled
        , Cancelled
    }

    public interface IOrderBase
    {
        string Id { get; }
        string ClientId { get; set; }
        DateTime CreatedAt { get; set; }
        double Volume { get; set; }
        double Price { get; set; }
        string AssetPairId { get; set; }
        string Status { get; set; }
        bool Straight { get; set; }
    }

    public static class BaseOrderExt
    {
        public const string Buy = "buy";
        public const string Sell = "sell";

        public static OrderType OrderAction(this IOrderBase orderBase)
        {
            return orderBase.Volume > 0 ? Exchange.OrderType.Buy : Exchange.OrderType.Sell;
        }

        public static OrderType? GetOrderAction(string actionWord)
        {
            if (actionWord.ToLower() == Buy)
                return Exchange.OrderType.Buy;
            if (actionWord.ToLower() == Sell)
                return Exchange.OrderType.Sell;

            return null;
        }

        public static OrderType ViceVersa(this OrderType orderType)
        {
            if (orderType == Exchange.OrderType.Buy)
                return Exchange.OrderType.Sell;
            return Exchange.OrderType.Buy;
        }
    }
}