using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.NFTs.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Queries.GetNFTClaims
{
    public class GetNFTClaimsQuery : IRequest<Result<List<NFTClaimDto>>>
    {

        public class GetNFTClaimsQueryHandler : IRequestHandler<GetNFTClaimsQuery, Result<List<NFTClaimDto>>>
        {
            private readonly ICallContext _context;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            public GetNFTClaimsQueryHandler(IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext)
            {
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<Result<List<NFTClaimDto>>> Handle(GetNFTClaimsQuery request, CancellationToken cancellationToken)
            {
                var receiverId = _context.UserId;
                var claims = await _dbContext.NFTClaims.AsQueryable()
                                    .Include(x => x.Sender)
                                    .Include(x => x.Receiver)
                                    .Where(x => x.Sender.IsAccountValid && x.ReceiverId == receiverId)
                                    .ToListAsync();

                var data = new List<NFTClaimDto>();
                foreach (var claim in claims)
                {
                    var sellOffers = _tokenService.GetNftSellOffers(claim.TokenId);

                    if (sellOffers.Offers != null && sellOffers.Offers.Any(x => x.Destination == claim.Receiver.AccountAddress))
                    {
                        var nft = _dbContext.NFTIndexes.AsQueryable().FirstOrDefault(x => x.TokenId == claim.TokenId && x.UriHex == claim.UriHex);

                        if (nft! != null)
                        {
                            var metadata = JsonConvert.DeserializeObject<NFTMetadata>(nft.Metadata);
                            var mappedClaim = new NFTClaimDto()
                            {
                                Id = claim.Id,
                                SenderMessage = claim.Message,
                                SenderUsername = claim.Sender.Username,
                                Metadata = metadata,
                                TokenId = claim.TokenId
                            };
                            data.Add(mappedClaim);
                        }
                    }
                }
                return await Result<List<NFTClaimDto>>.SuccessAsync(data);
            }
        }
    }
}
