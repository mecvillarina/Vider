using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.BurnNFT
{
    public class BurnNFTCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string TokenId { get; set; }

        public class BurnNFTCommandHandler : IRequestHandler<BurnNFTCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDomainEventService _domainEventService;
            private readonly IDateTime _dateTime;
            public BurnNFTCommandHandler(IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext, IDomainEventService domainEventService, IDateTime dateTime)
            {
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
                _domainEventService = domainEventService;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(BurnNFTCommand request, CancellationToken cancellationToken)
            {
                var nft = await _dbContext.NFTIndexes.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Id && x.TokenId == request.TokenId);

                if (nft == null) return await Result.FailAsync("No burnable NFT found.");

                var accountNftsResult = _tokenService.GetAccountNFTs(_context.UserAccountAddress);
                if (accountNftsResult.Status == "error") return await Result.FailAsync(accountNftsResult.ErrorMessage);

                if (!accountNftsResult.AccountNfts.Any(x => x.Uri == nft.UriHex && x.TokenId == nft.TokenId)) return await Result.FailAsync("No burnable NFT found.");

                var currentSellOffers = _tokenService.GetNftSellOffers(nft.TokenId);
                if (currentSellOffers.Offers != null)
                {
                    if (currentSellOffers.Offers.Any(x => x.Destination != null))
                    {
                        return await Result.FailAsync("Selected NFT has been gifted.");
                    }
                    else if (currentSellOffers.Offers.Any())
                    {
                        return await Result.FailAsync("Selected NFT is currently for sale. Please cancel the sell offer first.");
                    }
                }

                var burnResult = _tokenService.Burn(_context.UserAccountAddress, _context.UserAccountSecret, nft.TokenId);
                if (!burnResult.Succeeded) return await Result.FailAsync(burnResult.Messages);

                _dbContext.NFTIndexes.Remove(nft);
                await _dbContext.SaveChangesAsync();

                var metadata = JsonConvert.DeserializeObject<NFTMetadata>(nft.Metadata);
                await _domainEventService.Publish(new ActivityLogAddEvent(_context.UserId, _context.UserAccountAddress, $"You've burned an NFT ({metadata.Id}).", _dateTime.UtcNow, burnResult.Data));

                return await Result.SuccessAsync();
            }
        }
    }
}
