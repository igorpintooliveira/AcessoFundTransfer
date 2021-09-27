using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestBackend.FundTransfer.Application.Contracts.Business;
using TestBackend.FundTransfer.Application.Extensions;
using TestBackend.FundTransfer.Application.Models;
using TestBackend.FundTransfer.Application.Models.PartnerAcesso;

namespace TestBackend.FundTransfer.Application.Business
{
    public class PartnerAcessoBusiness : IPartnerAcessoBusiness
    {
        private readonly HttpClient _client;

        public PartnerAcessoBusiness(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResponseBase<AcessoAccount>> GetAccount(string accountNumber)
        {
            ResponseBase<AcessoAccount> returnResponse = new ResponseBase<AcessoAccount>();

            var response = await _client.GetAsync($"/api/Account/{accountNumber}");
            if (response.IsSuccessStatusCode)
            {
                returnResponse.Data = await response.ReadContentAs<AcessoAccount>();
            }
            else
            {
                returnResponse.Success = false;
                returnResponse.Message = response.ReasonPhrase;
            }

            return returnResponse;
        }

        public async Task<bool> SaveAccount(AcessoAccountPost acessoAccountPost)
        {
            var response = await _client.PostAsJson($"/api/Account", acessoAccountPost);
            return response.IsSuccessStatusCode;
        }
    }
}
