using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.ClaimNFT
{
    public class ClaimNFTCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string TokenId { get; set; }

        public class ClaimNFTCommandHandler : IRequestHandler<ClaimNFTCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;

            public ClaimNFTCommandHandler(IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext)
            {
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(ClaimNFTCommand request, CancellationToken cancellationToken)
            {
                var claim = await _dbContext.NFTClaims.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Id && x.TokenId == request.TokenId && x.ReceiverId == _context.UserId);

                if (claim == null) return await Result.FailAsync("No claimable NFT found.");

                var sellOffers = _tokenService.GetNftSellOffers(claim.TokenId);
                if (sellOffers == null || !sellOffers.Offers.Any(x => x.Destination == _context.UserAccountAddress))
                {
                    await RemoveNFTClaimsAsync(claim);
                    return await Result.FailAsync("No claimable NFT found.");
                }

                var accountSellOffer = sellOffers.Offers.Where(x => x.Destination == _context.UserAccountAddress).ToList();
                foreach (var sellOffer in accountSellOffer)
                {
                    var acceptSellOfferResult = _tokenService.AcceptSellOffer(_context.UserAccountAddress, _context.UserAccountSecret, sellOffer.Index);
                    if (!acceptSellOfferResult.Succeeded) return await Result.FailAsync(acceptSellOfferResult.Messages);
                }

                await RemoveNFTClaimsAsync(claim);
                return await Result.SuccessAsync();
            }

            private async Task RemoveNFTClaimsAsync(NFTClaim claim)
            {
                _dbContext.NFTClaims.Remove(claim);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
