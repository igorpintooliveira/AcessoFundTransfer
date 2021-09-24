using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Infrastructure.Mongo.Entities;

namespace TestBackend.FundTransfer.Infrastructure.Mongo.Data.Interfaces
{
    public interface IFundTransferContext
    {
        IMongoCollection<FundTransferMongoEntity> FundTransfers { get; }
    }
}
