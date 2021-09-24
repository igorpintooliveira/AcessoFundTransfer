using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Domain.Entities;
using TestBackend.FundTransfer.Infrastructure.Mongo.Entities;

namespace TestBackend.FundTransfer.Infrastructure.Mongo.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FundTransferEntity, FundTransferMongoEntity>().ReverseMap();
        }
    }
}
