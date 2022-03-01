using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Account.Queries.GetWallet
{
    public class GetWalletQuery : IRequest<Result<GetWalletResponse>>
    {
        public class GetAccountQueryHandler : IRequestHandler<GetWalletQuery, Result<GetWalletResponse>>
        {
            private readonly ICallContext _context;
            private readonly ICreatorIdentityService _identityService;
            private readonly IMapper _mapper;
            private readonly IXrplAccountService _accountService;
            public GetAccountQueryHandler(ICallContext context, ICreatorIdentityService identityService, IMapper mapper, IXrplAccountService accountService)
            {
                _context = context;
                _identityService = identityService;
                _mapper = mapper;
                _accountService = accountService;
            }

            public async Task<Result<GetWalletResponse>> Handle(GetWalletQuery request, CancellationToken cancellationToken)
            {
                var creator = await _identityService.GetAsync(_context.UserId);

                var accountInfo = _accountService.AccountInfo(creator.AccountAddress);

                double actualBalance = 0;
                if (accountInfo.Error == "actNotFound")
                {
                    creator.IsAccountValid = false;
                    await _identityService.UpdateWalletAsync(creator.Id, creator);
                    throw new UnauthorizedAccessException();
                }

                if (accountInfo.Status == "success")
                {
                    actualBalance = double.Parse(accountInfo.AccountData.Balance) / double.Parse(AppConstants.DropPerXRP.ToString());
                }
                else
                {
                    return await Result<GetWalletResponse>.FailAsync(accountInfo.ErrorMessage);
                }

                creator = await _identityService.GetAsync(_context.UserId);
                var mappedCreator = _mapper.Map<GetWalletResponse>(creator);
                mappedCreator.Balance = actualBalance;

                return await Result<GetWalletResponse>.SuccessAsync(mappedCreator);
            }
        }
    }
}
