using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackend.FundTransfer.Application.Models.PartnerAcesso
{
    public class AcessoAccountPost
    {
        public string accountNumber { get; set; }
        public decimal value { get; set; }
        public string type { get; set; }
    }
}
