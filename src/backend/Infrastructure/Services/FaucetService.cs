using Application.Common.Constants;
using Application.Common.Dtos.Response;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FaucetService : IFaucetService
    {
        private readonly IRestClient _client;
        public FaucetService(IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>(SettingKeys.FaucetEndpoint);
            _client = new RestClient(baseUrl);
        }

        public async Task<Result<FaucetResponseDto>> GenerateAccountAsync()
        {
            var request = new RestRequest("accounts", Method.POST);
            var response = _client.Execute<FaucetResponseDto>(request);
            if (!response.IsSuccessful) return await Result<FaucetResponseDto>.FailAsync("There's a problem on XRP Faucet Service. Please try again.");
            return await Result<FaucetResponseDto>.SuccessAsync(response.Data);
        }
    }
}
