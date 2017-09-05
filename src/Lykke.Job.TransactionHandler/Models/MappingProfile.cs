using AutoMapper;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using ServiceClientTrade = Lykke.Service.OperationsRepository.AutorestClient.Models.ClientTrade;
using ServiceTransferEvent = Lykke.Service.OperationsRepository.AutorestClient.Models.TransferEvent;

namespace Lykke.Job.TransactionHandler.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<IClientTrade, ServiceClientTrade>();
            CreateMap<ServiceClientTrade, IClientTrade>().As<ClientTrade>();
            CreateMap<TransferEvent, ServiceTransferEvent>();
        }
    }
}
