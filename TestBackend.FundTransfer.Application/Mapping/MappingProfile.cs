using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Command;
using TestBackend.FundTransfer.Application.Models;
using TestBackend.FundTransfer.Domain.Entities;
using TestBackend.FundTransfer.EventBus.Events;

namespace TestBackend.FundTransfer.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FundTransferRequest, FundTransferEntity>().ReverseMap();
            CreateMap<FundTransferEntity, FundTransferEvent>().ReverseMap();
            CreateMap<FundTransferResponseStatus, FundTransferEntity>().ReverseMap();
            CreateMap<FundTransferResponseId, FundTransferEntity>().ReverseMap();
            CreateMap<FundTransferEvent, FundTransferCommand>().ReverseMap();

        }
    }
}
