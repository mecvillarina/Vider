using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Account.Commands.Login
{
    public class LoginCommand : IRequest<Result<LoginCommandResponse>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
        {
            private readonly ICreatorIdentityService _identityService;

            public LoginCommandHandler(ICreatorIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var loginResult = await _identityService.LoginAsync(request.Username, request.Password);

                if (loginResult.Succeeded)
                {
                    return await Result<LoginCommandResponse>.SuccessAsync(new LoginCommandResponse()
                    {
                        Token = loginResult.Data.Token,
                        ExpireAt = loginResult.Data.ExpireAt
                    });
                }

                return await Result<LoginCommandResponse>.FailAsync(loginResult.Messages);
            }
        }
    }
}
