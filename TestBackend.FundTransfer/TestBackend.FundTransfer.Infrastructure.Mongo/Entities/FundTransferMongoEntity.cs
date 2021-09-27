using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackend.FundTransfer.Infrastructure.Mongo.Entities
{
    public class FundTransferMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string transactionId { get; set; }
        public string accountOrigin { get; set; }
        public string accountDestination { get; set; }
        public decimal value { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
}
