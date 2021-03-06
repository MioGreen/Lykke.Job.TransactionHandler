﻿using System;

namespace Lykke.Job.TransactionHandler.Core.Services.BitCoin.BitCoinApi.Models
{
    public class IssueData
    {
        public Guid? TransactionId { get; set; }
        public string Address { get; set; }
        public string AssetId { get; set; }
        public double Amount { get; set; }
    }

    public class TransferData
    {
        public Guid? TransactionId { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string AssetId { get; set; }
        public double Amount { get; set; }
    }

    public class TransferAllData
    {
        public Guid? TransactionId { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
    }

    public class DestroyData
    {
        public Guid? TransactionId { get; set; }
        public string Address { get; set; }
        public string AssetId { get; set; }
        public double Amount { get; set; }
    }

    public class SwapData
    {
        public Guid? TransactionId { get; set; }
        public string Multisig1 { get; set; }
        public string AssetId1 { get; set; }
        public double Amount1 { get; set; }
        public string Multisig2 { get; set; }
        public string AssetId2 { get; set; }
        public double Amount2 { get; set; }
    }

    public class RetryData
    {
        public Guid TransactionId { get; set; }
    }

    public class TransactionRepsonse
    {
        public Guid? TransactionId { get; set; }
    }

    public class OnchainResponse
    {
        public TransactionRepsonse Transaction { get; set; }
        public ErrorResponse Error { get; set; }
    }
}