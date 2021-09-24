using AutoMapper;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Command;
using TestBackend.FundTransfer.Application.Commom;
using TestBackend.FundTransfer.Application.Contracts.Business;
using TestBackend.FundTransfer.Application.Contracts.Infrastructure;
using TestBackend.FundTransfer.Application.Models;
using TestBackend.FundTransfer.Application.Models.PartnerAcesso;
using TestBackend.FundTransfer.Domain.Commom;
using TestBackend.FundTransfer.Domain.Entities;
using TestBackend.FundTransfer.EventBus.Events;

namespace TestBackend.FundTransfer.Application.Business
{
    public class FundTransferBusiness : IFundTransferBusiness
    {
        private readonly IFundTransferInQueue _fundTransferInQueue;
        private readonly IPartnerAcessoBusiness _partnerAcessoBusiness;
        private readonly IFundTransferRepository _fundTransferRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public FundTransferBusiness(IFundTransferInQueue fundTransferInQueue,
                                    IPartnerAcessoBusiness partnerAcessoBusiness,
                                    IFundTransferRepository fundTransferRepository,
                                    IMapper mapper,
                                    IPublishEndpoint publishEndpoint)
        {
            _fundTransferInQueue = fundTransferInQueue ?? throw new ArgumentNullException(nameof(fundTransferInQueue));
            _partnerAcessoBusiness = partnerAcessoBusiness ?? throw new ArgumentNullException(nameof(partnerAcessoBusiness));
            _fundTransferRepository = fundTransferRepository ?? throw new ArgumentNullException(nameof(fundTransferRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task<FundTransferResponseId> NewAsync(FundTransferRequest fundTransferRequest)
        {
            // Storage Redis
            var fundTransferEntity = _mapper.Map<FundTransferEntity>(fundTransferRequest);
            fundTransferEntity.transactionId = Guid.NewGuid().ToString();
            fundTransferEntity.status = FundTransferConstants.STATUS_IN_QUEUE;
            fundTransferEntity = await _fundTransferInQueue.UpdateAsync(fundTransferEntity);

            // Send event to rabbitmq
            var eventMessage = _mapper.Map<FundTransferEvent>(fundTransferEntity);
            await _publishEndpoint.Publish<FundTransferEvent>(eventMessage);

            // Return Id
            return _mapper.Map<FundTransferResponseId>(fundTransferEntity);
        }

        public async Task<FundTransferEntity> GetAsync(string transactionId)
        {
            var fundTransferEntity = await _fundTransferInQueue.GetAsync(transactionId);

            if (fundTransferEntity == null)
                fundTransferEntity = await _fundTransferRepository.GetAsync(transactionId);

            return fundTransferEntity;
        }

        public async Task<FundTransferResponseStatus> GetStatusAsync(string transactionId)
        {
            var fundTransferEntity = await _fundTransferInQueue.GetAsync(transactionId);

            if (fundTransferEntity == null)
                fundTransferEntity = await _fundTransferRepository.GetAsync(transactionId);

            return  _mapper.Map<FundTransferResponseStatus>(fundTransferEntity);
        }

        public async Task<FundTransferEntity> ExecuterTransferAsync(FundTransferCommand fundTransferCommand)
        {
            // IGOR -> TO DO
            // basic rule ...needs more attention on erros, rules and mutch more ... needs more definitions

            // Change status on redis to Processing
            await executeTransferChangeCacheStatus(fundTransferCommand, FundTransferConstants.STATUS_PROCESSING);

            // Exists account origin ?
            var responseAccountOrigin = await _partnerAcessoBusiness.GetAccount(fundTransferCommand.accountOrigin);
            if (!responseAccountOrigin.Success)
                return await executeTransferErrorAction(fundTransferCommand, responseAccountOrigin.Message);

            // Exists account destination ?
            var responseAccountDestination = await _partnerAcessoBusiness.GetAccount(fundTransferCommand.accountDestination);
            if (!responseAccountDestination.Success)
                return await executeTransferErrorAction(fundTransferCommand, responseAccountDestination.Message);

            // IGOR -> TO DO
            // needs more attention and implementation
            // rules when debit success and credit not ... it's not defined on test scope                     

            var responseSaveDebit = await executeTransferDebit(fundTransferCommand);
            if (!responseSaveDebit.Success)
                return await executeTransferErrorAction(fundTransferCommand, responseSaveDebit.Message);

            var responseSaveCredit = await executeTransferCredit(fundTransferCommand);
            if (!responseSaveCredit.Success)
                return await executeTransferErrorAction(fundTransferCommand, responseSaveCredit.Message);


            // manage queue on erros


            // Return Id
            return await executeTransferSuccessAction(fundTransferCommand);
        }

        private async Task<FundTransferEntity> executeTransferChangeCacheStatus(FundTransferCommand fundTransferCommand,
                                                                                             string fundTransferStatus)
        {
            FundTransferEntity fundTransferEntity = await _fundTransferInQueue.GetAsync(fundTransferCommand.transactionId);
            fundTransferEntity.status = fundTransferStatus;
            fundTransferEntity = await _fundTransferInQueue.UpdateAsync(fundTransferEntity);

            return fundTransferEntity;
        }

        private async Task executeTransferRemoveCache(FundTransferCommand fundTransferCommand)
        {
            await _fundTransferInQueue.DeleteAsync(fundTransferCommand.transactionId);
        }
        private async Task executeTransferRemoveCache(FundTransferEntity fundTransferEntity)
        {
            await _fundTransferInQueue.DeleteAsync(fundTransferEntity.transactionId);
        }

        private async Task<ResponseBase<AcessoAccountPost>> executeTransferDebit(FundTransferCommand fundTransferCommand)
        {
            var accountOriginPost = new AcessoAccountPost
            {
                accountNumber = fundTransferCommand.accountOrigin,
                value = fundTransferCommand.value,
                type = PartnerAcessoConstants.TRANSFER_TYPE_DEBIT
            };

            bool success = await _partnerAcessoBusiness.SaveAccount(accountOriginPost);

            var returnResponse = new ResponseBase<AcessoAccountPost>();
            returnResponse.Data = accountOriginPost;
            returnResponse.Success = success;
            if (!success)
                returnResponse.Message = $"Error on process debit in accountNumber {accountOriginPost.accountNumber} value {accountOriginPost.value}";

            return returnResponse;
        }

        private async Task<ResponseBase<AcessoAccountPost>> executeTransferCredit(FundTransferCommand fundTransferCommand)
        {
            var accountDestinationPost = new AcessoAccountPost
            {
                accountNumber = fundTransferCommand.accountDestination,
                value = fundTransferCommand.value,
                type = PartnerAcessoConstants.TRANSFER_TYPE_CREDIT
            };

            bool success = await _partnerAcessoBusiness.SaveAccount(accountDestinationPost);

            var returnResponse = new ResponseBase<AcessoAccountPost>();
            returnResponse.Data = accountDestinationPost;
            returnResponse.Success = success;
            if (!success)
                returnResponse.Message = $"Error on process credit in accountNumber {accountDestinationPost.accountNumber} value {accountDestinationPost.value}";

            return returnResponse;
        }

        private async Task<FundTransferEntity> executeTransferErrorAction(FundTransferCommand fundTransferCommand,
                                                                    string message)
        {
            FundTransferEntity fundTransferEntity = await _fundTransferInQueue.GetAsync(fundTransferCommand.transactionId);
            fundTransferEntity.status = FundTransferConstants.STATUS_ERROR;
            fundTransferEntity.message = message;

            await executeTransferPersist(fundTransferEntity);

            await executeTransferRemoveCache(fundTransferCommand);

            return fundTransferEntity;
        }

        private async Task<FundTransferEntity> executeTransferSuccessAction(FundTransferCommand fundTransferCommand)
        {
            // Change status on redis to confirmed
            FundTransferEntity fundTransferEntity = await executeTransferChangeCacheStatus(fundTransferCommand, FundTransferConstants.STATUS_CONFIRMED);
                        
            await executeTransferPersist(fundTransferEntity);

            await executeTransferRemoveCache(fundTransferCommand);

            return fundTransferEntity;
        }

        private async Task<FundTransferEntity> executeTransferPersist(FundTransferEntity fundTransferEntity)
        {
            await _fundTransferRepository.CreateAsync(fundTransferEntity);

            await executeTransferRemoveCache(fundTransferEntity);

            return fundTransferEntity;
        }


    }
}
