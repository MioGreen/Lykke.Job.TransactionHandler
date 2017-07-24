using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Clients
{
    public class PersonalDataEntity : TableEntity, IFullPersonalData
    {
        public static string GeneratePartitionKey()
        {
            return "PD";
        }

        public static string GenerateRowKey(string clientId)
        {
            return clientId;
        }


        public DateTime Regitered { get; set; }
        public string Id => RowKey;
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string CountryFromPOA { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string ReferralCode { get; set; }

        public string PasswordHint { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CountryFromID { get; set; }

        public string SpotRegulator { get; set; }

        public string MarginRegulator { get; set; }
        public string PaymentSystem { get; set; }

        internal void Update(IPersonalData src)
        {
            Country = src.Country;
            Zip = src.Zip;
            City = src.City;
            Address = src.Address;
            ContactPhone = src.ContactPhone;
            FullName = src.FullName;
            FirstName = src.FirstName;
            LastName = src.LastName;
            SpotRegulator = src.SpotRegulator;
        }

        public static PersonalDataEntity Create(IPersonalData src)
        {
            var result = new PersonalDataEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                Email = src.Email,
                Regitered = src.Regitered
            };

            result.Update(src);

            return result;
        }

        public static PersonalDataEntity Create(IFullPersonalData src)
        {
            var result = Create((IPersonalData)src);

            result.PasswordHint = src.PasswordHint;

            return result;
        }
    }

    public class PersonalDataRepository : IPersonalDataRepository
    {
        private readonly INoSQLTableStorage<PersonalDataEntity> _tableStorage;

        public PersonalDataRepository(INoSQLTableStorage<PersonalDataEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IPersonalData> GetAsync(string id)
        {
            var partitionKey = PersonalDataEntity.GeneratePartitionKey();
            var rowKey = PersonalDataEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IPersonalData>> GetAsync(IEnumerable<string> id)
        {
            var partitionKey = PersonalDataEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey, id);
        }

        public async Task GetByChunksAsync(Action<IEnumerable<IPersonalData>> callback)
        {
            var partitionKey = PersonalDataEntity.GeneratePartitionKey();

            await _tableStorage.GetDataByChunksAsync(partitionKey, callback);
        }

        public Task UpdateGeolocationDataAsync(string id, string countryCode, string city)
        {
            var partitionKey = PersonalDataEntity.GeneratePartitionKey();
            var rowKey = PersonalDataEntity.GenerateRowKey(id);

            return _tableStorage.MergeAsync(partitionKey, rowKey, itm =>
            {
                if (itm.Country == null)
                {
                    itm.Country = countryCode;
                }
                if (itm.CountryFromPOA == null)
                {
                    itm.CountryFromPOA = countryCode;
                }
                itm.City = city;
                return itm;
            });
        }

        public Task SetReferralCode(string id, string refCode)
        {
            var partitionKey = PersonalDataEntity.GeneratePartitionKey();
            var rowKey = PersonalDataEntity.GenerateRowKey(id);

            return _tableStorage.MergeAsync(partitionKey, rowKey, itm =>
            {
                itm.ReferralCode = refCode;
                return itm;
            });
        }
    }

}