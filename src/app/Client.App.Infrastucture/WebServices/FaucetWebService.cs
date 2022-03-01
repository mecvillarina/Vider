using Application.Common.Dtos.Response;
using Application.Common.Models;
using Application.CreatorPortal.Faucet.Commands;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class FaucetWebService : WebServiceBase, IFaucetWebService
    {
        public FaucetWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<FaucetResponseDto>> GenerateAccountAsync(GenerateAccountCommand request) => PostAsync<GenerateAccountCommand, FaucetResponseDto>(FaucetEndpoints.GenerateAccount, request);
    }
}
