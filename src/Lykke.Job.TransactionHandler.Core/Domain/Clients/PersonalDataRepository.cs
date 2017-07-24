using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Clients
{
    public interface IPersonalData
    {
        DateTime Regitered { get; }
        string Id { get; }
        string Email { get; }
        string FullName { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// ISO Alpha 2 code
        /// </summary>
        string CountryFromID { get; set; }


        /// <summary>
        /// ISO Alpha 3 code
        /// </summary>
        string Country { get; set; }
        /// <summary>
        /// ISO Alpha 3 code. Country from Proof of Address.
        /// </summary>
        string CountryFromPOA { get; set; }

        string Zip { get; set; }
        string City { get; }
        string Address { get; }
        string ContactPhone { get; }
        string ReferralCode { get; }
        string SpotRegulator { get; }
        string MarginRegulator { get; }
        string PaymentSystem { get; }
    }

    public class PersonalData : IPersonalData
    {
        public DateTime Regitered { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// ISO Alpha 2 code
        /// </summary>
        public string CountryFromID { get; set; }
        /// <summary>
        /// ISO Alpha 3 code
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// ISO Alpha 3 code
        /// </summary>
        public string CountryFromPOA { get; set; }

        public string Zip { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string ReferralCode { get; set; }
        public string SpotRegulator { get; set; }
        public string MarginRegulator { get; set; }
        public string PaymentSystem { get; set; }
    }

    public interface IFullPersonalData : IPersonalData
    {
        string PasswordHint { get; set; }
    }

    public interface ISearchPersonalData : IPersonalData
    {
        List<IPersonalData> OtherClients { get; }
    }

    public class FullPersonalData : IFullPersonalData
    {
        public DateTime Regitered { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CountryFromID { get; set; }
        public string Country { get; set; }
        public string CountryFromPOA { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string ReferralCode { get; set; }
        public string PasswordHint { get; set; }
        public string SpotRegulator { get; set; }
        public string MarginRegulator { get; set; }
        public string PaymentSystem { get; set; }

        public static FullPersonalData Create(IClientAccount src, string fullName, string pwdHint)
        {
            return new FullPersonalData
            {
                Id = src.Id,
                Email = src.Email,
                ContactPhone = src.Phone,
                Regitered = src.Registered,
                FullName = fullName,
                Country = "CHE",
                PasswordHint = pwdHint
            };
        }
    }

    public class SearchPersonalData : FullPersonalData, ISearchPersonalData
    {
        /// <summary> Only for serialization </summary>
        public List<PersonalData> OtherClients { get; set; }

        List<IPersonalData> ISearchPersonalData.OtherClients => OtherClients?.Cast<IPersonalData>().ToList();

        public static SearchPersonalData Create(IPersonalData personalData)
        {
            return new SearchPersonalData
            {
                Id = personalData.Id,
                Email = personalData.Email,
                Regitered = personalData.Regitered,
                Country = personalData.Country,
                CountryFromID = personalData.CountryFromID,
                CountryFromPOA = personalData.CountryFromPOA,
                Zip = personalData.Zip,
                City = personalData.City,
                Address = personalData.Address,
                ContactPhone = personalData.ContactPhone,
                FullName = personalData.FullName,
                FirstName = personalData.FirstName,
                LastName = personalData.LastName,
                ReferralCode = personalData.ReferralCode,
                SpotRegulator = personalData.SpotRegulator,
                MarginRegulator = personalData.MarginRegulator,
                PaymentSystem = personalData.PaymentSystem,
            };
        }
    }

    //TODO: remove and use IPersonalDataService
    public interface IPersonalDataRepository
    {
        Task<IPersonalData> GetAsync(string id);
        Task<IEnumerable<IPersonalData>> GetAsync(IEnumerable<string> id);
        Task GetByChunksAsync(Action<IEnumerable<IPersonalData>> callback);
        Task UpdateGeolocationDataAsync(string id, string countryCode, string city);
        Task SetReferralCode(string id, string refCode);
    }
}