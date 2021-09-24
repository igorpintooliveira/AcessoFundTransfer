using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Domain.Entities;

namespace TestBackend.FundTransfer.Application.Contracts.Infrastructure
{
    public interface IFundTransferInQueue
    {
        Task<FundTransferEntity> GetAsync(string transactionId);
        Task<FundTransferEntity> UpdateAsync(FundTransferEntity fundTransferEntity);
        Task DeleteAsync(string userName);
    }
}
