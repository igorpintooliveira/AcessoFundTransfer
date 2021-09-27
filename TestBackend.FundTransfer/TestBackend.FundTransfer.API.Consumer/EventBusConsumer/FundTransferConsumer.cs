using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Command;
using TestBackend.FundTransfer.Domain.Entities;
using TestBackend.FundTransfer.EventBus.Events;

namespace TestBackend.FundTransfer.API.Consumer.EventBusConsumer
{
    public class FundTransferConsumer : IConsumer<FundTransferEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<FundTransferConsumer> _logger;

        public FundTransferConsumer(IMediator mediator, 
                                    IMapper mapper,
                                    ILogger<FundTransferConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<FundTransferEvent> context)
        {
            FundTransferEntity fundTransferEntityResult = null;
            try
            {
                var command = _mapper.Map<FundTransferCommand>(context.Message);
                fundTransferEntityResult = await _mediator.Send(command);
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
            }

            _logger.LogInformation($"FundTransferEvent consumed successfully. Fund-Transfer transactionId : {fundTransferEntityResult.transactionId}");
        }
    }
}
