using Application.Common.Dtos.Response;
using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IFaucetService
    {
        Task<Result<FaucetResponseDto>> GenerateAccountAsync();
    }
}
