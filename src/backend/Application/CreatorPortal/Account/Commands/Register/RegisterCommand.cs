using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Account.Commands.Register
{
    public class RegisterCommand : IRequest<IResult>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string AccountXAddress { get; set; }
        public string AccountSecret { get; set; }
        public string AccountClassicAddress { get; set; }
        public string AccountAddress { get; set; }
        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IResult>
        {
            private readonly ICreatorIdentityService _identityService;

            public RegisterCommandHandler(ICreatorIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<IResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var creator = new Creator()
                {
                    Username = request.Username,
                    Bio = request.Bio,
                    Name = request.Name,
                    AccountXAddress = request.AccountXAddress,
                    AccountSecret = request.AccountSecret,
                    AccountClassicAddress = request.AccountClassicAddress,
                    AccountAddress = request.AccountAddress,
                    IsAccountValid = true,
                };

                var createResult = await _identityService.CreateAsync(creator);

                if (createResult.Succeeded)
                {
                    await _identityService.SetPasswordAsync(createResult.Data, request.Password);
                    return await Result.SuccessAsync();
                }

                return await Result.FailAsync(createResult.Messages);
            }
        }
    }
}
