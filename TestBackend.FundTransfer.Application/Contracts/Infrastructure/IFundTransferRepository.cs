using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Domain.Entities;

namespace TestBackend.FundTransfer.Application.Contracts.Infrastructure
{
    public interface IFundTransferRepository
    {
        Task<IEnumerable<FundTransferEntity>> ListAsync();
        Task<FundTransferEntity> GetAsync(string transactionId);
        Task CreateAsync(FundTransferEntity fundTransferEntity);
        Task<bool> UpdateAsync(FundTransferEntity fundTransferEntity);
        Task<bool> DeleteAsync(string transactionId);
    }
}
