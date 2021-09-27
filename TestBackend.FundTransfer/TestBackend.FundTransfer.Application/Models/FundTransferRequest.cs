using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackend.FundTransfer.Application.Models
{
    public class FundTransferRequest
    {
        public string accountOrigin { get; set; }
        public string accountDestination { get; set; }
        public decimal value { get; set; }
    }
}
