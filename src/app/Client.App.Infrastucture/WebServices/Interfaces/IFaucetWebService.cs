using Application.Common.Dtos.Response;
using Application.Common.Models;
using Application.CreatorPortal.Faucet.Commands;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IFaucetWebService : IWebService
    {
        Task<IResult<FaucetResponseDto>> GenerateAccountAsync(GenerateAccountCommand request);
    }
}