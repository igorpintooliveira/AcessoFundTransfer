using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Command;
using TestBackend.FundTransfer.Application.Models;
using TestBackend.FundTransfer.Domain.Entities;

namespace TestBackend.FundTransfer.Application.Contracts.Business
{
    public interface IFundTransferBusiness
    {
        Task<FundTransferResponseId> NewAsync(FundTransferRequest fundTransferRequest);
        Task<FundTransferEntity> GetAsync(string transactionId);
        Task<FundTransferResponseStatus> GetStatusAsync(string transactionId);
        Task<FundTransferEntity> ExecuterTransferAsync(FundTransferCommand fundTransferCommand);
    }
}
