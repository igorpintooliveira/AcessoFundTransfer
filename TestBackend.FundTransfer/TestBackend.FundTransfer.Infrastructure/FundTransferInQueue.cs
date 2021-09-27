using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Contracts.Infrastructure;
using TestBackend.FundTransfer.Domain.Entities;

namespace TestBackend.FundTransfer.Infrastructure.Redis
{
    public class FundTransferInQueue : IFundTransferInQueue
    {
        private readonly IDistributedCache _redisCache;

        public FundTransferInQueue(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<FundTransferEntity> GetAsync(string transactionId)
        {
            var fundTransferEntity = await _redisCache.GetStringAsync(transactionId);

            if (String.IsNullOrEmpty(fundTransferEntity))
                return null;

            return JsonConvert.DeserializeObject<FundTransferEntity>(fundTransferEntity);
        }

        public async Task<FundTransferEntity> UpdateAsync(FundTransferEntity fundTransferEntity)
        {
            await _redisCache.SetStringAsync(fundTransferEntity.transactionId, JsonConvert.SerializeObject(fundTransferEntity));

            return await GetAsync(fundTransferEntity.transactionId);
        }

        public async Task DeleteAsync(string transactionId)
        {
            await _redisCache.RemoveAsync(transactionId);
        }
    }
}
