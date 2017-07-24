using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;

namespace Lykke.Job.TransactionHandler.Core.Services.SolarCoin
{
    public interface ISrvSolarCoinHelper
    {
        Task<string> SetNewSolarCoinAddress(IWalletCredentials walletCredentials);
        Task SendCashOutRequest(string id, SolarCoinAddress addressTo, double amount);
    }

    public class SolarCoinAddress
    {
        private string _address;
        private static readonly Regex Base58Regex = new Regex(@"^[123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz]+$");

        public string Value
        {
            get { return _address; }
            set
            {
                SetAddress(value);
            }
        }

        public SolarCoinAddress(string address)
        {
            SetAddress(address);
        }

        private void SetAddress(string address)
        {
            if (!IsValid(address))
                throw new ArgumentException("Address is invalid");

            _address = address;
        }

        public static bool IsValid(string address)
        {
            return !string.IsNullOrEmpty(address) && address[0] == '8' && address.Length < 40 && Base58Regex.IsMatch(address);
        }
    }
}