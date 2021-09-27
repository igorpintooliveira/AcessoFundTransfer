using TestBackend.FundTransfer.Infrastructure.Mongo.Data.Interfaces;
using MongoDB.Driver;
using TestBackend.FundTransfer.Infrastructure.Mongo.Entities;
using Microsoft.Extensions.Configuration;

namespace TestBackend.FundTransfer.Infrastructure.Mongo.Data
{
    public class FundTransferContext : IFundTransferContext
    {
        public FundTransferContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("MongoDatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDatabaseSettings:DatabaseName"));

            FundTransfers = database.GetCollection<FundTransferMongoEntity>(configuration.GetSection("MongoDatabaseSettings:CollectionName").Value);            
        }

        public IMongoCollection<FundTransferMongoEntity> FundTransfers { get; }
    }
}
