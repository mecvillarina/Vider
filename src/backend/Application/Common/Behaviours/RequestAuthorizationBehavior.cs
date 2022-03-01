using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace Application.Common.Behaviours
{
    public class RequestAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        private readonly IAuthTokenService _authTokenService;
        private readonly ICallContext _context;
        private readonly ICreatorIdentityService _identityService;
        public RequestAuthorizationBehavior(ICallContext context,
            IAuthTokenService authTokenService, ICreatorIdentityService identityService)
        {
            _context = context;
            _authTokenService = authTokenService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_context.UserRequiresAuthorization)
            {
                if (string.IsNullOrEmpty(_context.UserBearerAuthorizationToken)) throw new UnauthorizedAccessException();

                var authTokenResult = _authTokenService.ValidateToken(_context.UserBearerAuthorizationToken);
                if (authTokenResult.Status != AuthTokenStatus.Valid) throw new UnauthorizedAccessException();

                var nameIdentifier = authTokenResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (nameIdentifier == null) throw new UnauthorizedAccessException();

                _context.UserId = Convert.ToInt32(nameIdentifier.Value);

                var creator = await _identityService.GetAsync(_context.UserId);

                if (creator == null || !creator.IsAccountValid) throw new UnauthorizedAccessException();
                _context.Username = creator.Username;
                _context.UserAccountXAddress = creator.AccountXAddress;
                _context.UserAccountSecret = AESExtensions.Decrypt(creator.AccountSecret, creator.Salt);
                _context.UserAccountClassicAddress = creator.AccountClassicAddress;
                _context.UserAccountAddress = creator.AccountAddress;
            }

            return await next();
        }
    }
}