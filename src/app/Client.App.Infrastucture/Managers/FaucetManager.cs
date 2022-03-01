using Application.Common.Dtos.Response;
using Application.Common.Models;
using Application.CreatorPortal.Faucet.Commands;
using Client.App.Infrastructure.WebServices;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class FaucetManager : ManagerBase, IFaucetManager
    {
        private readonly IFaucetWebService _faucetWebService;
        public FaucetManager(IManagerToolkit managerToolkit, IFaucetWebService faucetWebService) : base(managerToolkit)
        {
            _faucetWebService = faucetWebService;
        }

        public async Task<IResult<FaucetResponseDto>> GenerateAccountAsync()
        {
            return await _faucetWebService.GenerateAccountAsync(new GenerateAccountCommand());
        }
    }
}
