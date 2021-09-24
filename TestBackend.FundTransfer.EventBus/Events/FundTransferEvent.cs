using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackend.FundTransfer.EventBus.Events
{
    public class FundTransferEvent : IntegrationBaseEvent
    {
        public string transactionId { get; set; }
        public string accountOrigin { get; set; }
        public string accountDestination { get; set; }
        public decimal value { get; set; }        
    }
}
