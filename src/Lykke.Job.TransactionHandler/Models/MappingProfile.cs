using AutoMapper;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using ServiceClientTrade = Lykke.Service.OperationsRepository.AutorestClient.Models.ClientTrade;

namespace Lykke.Job.TransactionHandler.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<IClientTrade, ServiceClientTrade>();
            CreateMap<ServiceClientTrade, IClientTrade>().As<ClientTrade>();
        }
    }
}
