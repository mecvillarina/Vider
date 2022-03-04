using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using Application.CreatorPortal.Faucet.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queues.Commands.WalletChecker
{
    public class WalletCheckerQueueCommand : IRequest<IResult>
    {
        public WalletCheckerQueueMessage Message { get; set; }

        public class WalletCheckerQueueCommandHandler : IRequestHandler<WalletCheckerQueueCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;
            private readonly IDateTime _dateTime;
            private readonly IXrplAccountService _accountService;
            private readonly ICreatorIdentityService _identityService;

            public WalletCheckerQueueCommandHandler(IApplicationDbContext dbContext, IMediator mediator, IDateTime dateTime, IXrplAccountService accountService, ICreatorIdentityService identityService)
            {
                _dbContext = dbContext;
                _mediator = mediator;
                _dateTime = dateTime;
                _accountService = accountService;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(WalletCheckerQueueCommand request, CancellationToken cancellationToken)
            {
                if (request.Message == null) await Result.FailAsync();

                var creator = await _dbContext.Creators.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Message.CreatorId);

                if (creator != null)
                {
                    var userAccountInfo = _accountService.AccountInfo(creator.AccountAddress);
                    if (userAccountInfo.Status == "error")
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
                            _identityService.UpdateWalletAsync(creator.Id, creator).Wait();
                        }
                    }
                }

                return await Result.SuccessAsync();
            }
        }
    }
}
