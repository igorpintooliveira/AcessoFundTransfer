using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Contracts.Infrastructure;
using TestBackend.FundTransfer.Domain.Entities;
using TestBackend.FundTransfer.Infrastructure.Mongo.Data.Interfaces;
using TestBackend.FundTransfer.Infrastructure.Mongo.Entities;

namespace TestBackend.FundTransfer.Infrastructure.Mongo.Repositories
{
    public class FundTransferRepository : IFundTransferRepository
    {
        private readonly IFundTransferContext _context;
        private readonly IMapper _mapper;

        public FundTransferRepository(IFundTransferContext fundTransferContext,
                                      IMapper mapper)
        {
            _context = fundTransferContext ?? throw new ArgumentNullException(nameof(fundTransferContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<FundTransferEntity>> ListAsync()
        {
            return await _context
                            .FundTransfers
                            .Find(x => true)
                            .Project(Builders<FundTransferMongoEntity>.Projection.As<FundTransferEntity>())
                            .ToListAsync();
        }

        public async Task<FundTransferEntity> GetAsync(string transactionId)
        {

            return await _context
                           .FundTransfers
                           .Find(x => x.transactionId == transactionId)
                           .Project<FundTransferEntity>(Builders<FundTransferMongoEntity>.Projection.Exclude(f => f.id))
                           //.Project(Builders<FundTransferMongoEntity>.Projection.As<FundTransferEntity>())
                           .FirstOrDefaultAsync();
        }


        public async Task CreateAsync(FundTransferEntity fundTransferEntity)
        {
            var fundTransferMongoEntity = _mapper.Map<FundTransferMongoEntity>(fundTransferEntity);
            await _context.FundTransfers.InsertOneAsync(fundTransferMongoEntity);
        }

        public async Task<bool> UpdateAsync(FundTransferEntity fundTransferEntity)
        {
            var fundTransferMongoEntity = _mapper.Map<FundTransferMongoEntity>(fundTransferEntity);

            var updateResult = await _context
                                        .FundTransfers
                                        .ReplaceOneAsync(filter: x => x.transactionId == fundTransferEntity.transactionId, replacement: fundTransferMongoEntity);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string transactionId)
        {
            FilterDefinition<FundTransferMongoEntity> filter = Builders<FundTransferMongoEntity>.Filter.Eq(x => x.transactionId, transactionId);

            DeleteResult deleteResult = await _context
                                                .FundTransfers
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
