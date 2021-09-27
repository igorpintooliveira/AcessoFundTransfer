using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Domain.Entities;

namespace TestBackend.FundTransfer.Application.Command
{
    public class FundTransferCommand : IRequest<FundTransferEntity>
    {
        public string transactionId { get; set; }
        public string accountOrigin { get; set; }
        public string accountDestination { get; set; }
        public decimal value { get; set; }
    }
}
