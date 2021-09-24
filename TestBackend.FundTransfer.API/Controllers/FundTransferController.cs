using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Contracts.Business;
using TestBackend.FundTransfer.Application.Models;

namespace TestBackend.FundTransfer.API.Controllers
{
    [Route("api/fund-transfer")]
    [ApiController]
    public class FundTransferController : ControllerBase
    {
        private readonly IFundTransferBusiness _fundTransferBusiness;

        public FundTransferController(IFundTransferBusiness fundTransferBusiness)
        {
            _fundTransferBusiness = fundTransferBusiness ?? throw new ArgumentNullException(nameof(fundTransferBusiness));            
        }

        [HttpPost]
        [ProducesResponseType(typeof(FundTransferResponseStatus), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FundTransferResponseId>> Post([FromBody] FundTransferRequest fundTransferRequest)
        {
            return await _fundTransferBusiness.NewAsync(fundTransferRequest);
        }

        [HttpGet("{transactionId}")]
        [ProducesResponseType(typeof(FundTransferResponseId), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FundTransferResponseStatus>> Get(string transactionId)
        {
            return await _fundTransferBusiness.GetStatusAsync(transactionId);
        }
    }
}
