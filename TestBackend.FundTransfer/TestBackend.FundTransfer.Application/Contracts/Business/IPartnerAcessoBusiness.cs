using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Models;
using TestBackend.FundTransfer.Application.Models.PartnerAcesso;

namespace TestBackend.FundTransfer.Application.Contracts.Business
{
    public interface IPartnerAcessoBusiness
    {
        Task<ResponseBase<AcessoAccount>> GetAccount(string accountNumber);
        Task<bool> SaveAccount(AcessoAccountPost acessoAccountPost);
    }
}
