using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Contracts.Business;
using TestBackend.FundTransfer.Domain.Entities;

namespace TestBackend.FundTransfer.Application.Command
{
    public class FundTransferCommandHandler : IRequestHandler<FundTransferCommand, FundTransferEntity>
    {
        private readonly IFundTransferBusiness _fundTransferBusiness;
        private readonly IMapper _mapper;

        public FundTransferCommandHandler(IFundTransferBusiness fundTransferBusiness, 
                                          IMapper mapper)
        {
            _fundTransferBusiness = fundTransferBusiness ?? throw new ArgumentNullException(nameof(fundTransferBusiness));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<FundTransferEntity> Handle(FundTransferCommand fundTransferCommand, CancellationToken cancellationToken)
        {
            // IGOR -> TO DO
            // needs more attention and implementation

            return await _fundTransferBusiness.ExecuterTransferAsync(fundTransferCommand);


            // criar logica para fazer a transferência usando api da acesso
            //var orderEntity = _mapper.Map<Order>(request);
            //var newOrder = await _orderRepository.AddAsync(orderEntity);

            //_logger.LogInformation($"Order {newOrder.Id} is successfully created.");

           
        }
    }
}
