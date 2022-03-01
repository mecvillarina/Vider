using Application.Common.Dtos.Response;
using Application.Common.Models;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IFaucetManager : IManager
    {
        Task<IResult<FaucetResponseDto>> GenerateAccountAsync();
    }
}