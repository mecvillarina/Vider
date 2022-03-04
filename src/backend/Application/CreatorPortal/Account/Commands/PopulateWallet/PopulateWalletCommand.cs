using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Faucet.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Account.Commands.PopulateWallet
{
    public class PopulateWalletCommand : IRequest<IResult>
    {
        public class PopulateWalletCommandHandler : IRequestHandler<PopulateWalletCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly ICreatorIdentityService _identityService;
            private readonly IMediator _mediator;
            private readonly IDateTime _dateTime;
            public PopulateWalletCommandHandler(ICallContext context, ICreatorIdentityService identityService, IMediator mediator, IDateTime dateTime)
            {
                _context = context;
                _identityService = identityService;
                _mediator = mediator;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(PopulateWalletCommand request, CancellationToken cancellationToken)
            {
                var creator = await _identityService.GetAsync(_context.UserId);

                if (creator != null)
                {
                    var faucetWallet = await _mediator.Send(new GenerateAccountCommand(), cancellationToken);

                    if (faucetWallet.Succeeded)
                    {
                        creator.IsAccountValid = true;
                        creator.AccountAddress = faucetWallet.Data.Account.Address;
                        creator.AccountSecret = faucetWallet.Data.Account.Secret;
                        creator.AccountXAddress = faucetWallet.Data.Account.XAddress;
                        creator.AccountClassicAddress = faucetWallet.Data.Account.ClassicAddress;
                        creator.DateAccountAcquired = _dateTime.UtcNow;
                        return await _identityService.UpdateWalletAsync(_context.UserId, creator);
                    }
                }

                return await Result.FailAsync();
            }
        }
    }
}
