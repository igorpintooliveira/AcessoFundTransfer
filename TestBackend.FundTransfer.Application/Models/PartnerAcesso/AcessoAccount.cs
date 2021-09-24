using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackend.FundTransfer.Application.Models.PartnerAcesso
{
    public class AcessoAccount
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public decimal balance { get; set; }
    }
}
