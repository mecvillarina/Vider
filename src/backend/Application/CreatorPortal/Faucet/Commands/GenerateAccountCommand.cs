using Application.Common.Dtos.Response;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Faucet.Commands
{
    public class GenerateAccountCommand : IRequest<Result<FaucetResponseDto>>
    {
        public class GenerateAccountCommandHandler : IRequestHandler<GenerateAccountCommand, Result<FaucetResponseDto>>
        {
            private readonly IFaucetService _faucetService;

            public GenerateAccountCommandHandler(IFaucetService faucetService)
            {
                _faucetService = faucetService;
            }

            public Task<Result<FaucetResponseDto>> Handle(GenerateAccountCommand request, CancellationToken cancellationToken)
            {
                return _faucetService.GenerateAccountAsync();
            }
        }
    }
}
