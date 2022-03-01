using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.CancelSellNFT
{
    public class CancelSellNFTCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string TokenId { get; set; }

        public class CancelSellNFTCommandHandler : IRequestHandler<CancelSellNFTCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDateTime _dateTime;
            public CancelSellNFTCommandHandler(IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext, IDateTime dateTime)
            {
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(CancelSellNFTCommand request, CancellationToken cancellationToken)
            {
                var nft = await _dbContext.NFTIndexes.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Id && x.TokenId == request.TokenId);

                if (nft == null) return await Result.FailAsync("No NFT found.");

                var accountNftsResult = _tokenService.GetAccountNFTs(_context.UserAccountAddress);
                if (accountNftsResult.Status == "error") return await Result.FailAsync(accountNftsResult.ErrorMessage);

                if (!accountNftsResult.AccountNfts.Any(x => x.Uri == nft.UriHex && x.TokenId == nft.TokenId)) return await Result.FailAsync("No NFT found.");

                var currentSellOffers = _tokenService.GetNftSellOffers(nft.TokenId);
                if (currentSellOffers.Offers == null || !currentSellOffers.Offers.Any())
                {
                    await RemoveIndexSellOffersAsync(request.TokenId);
                    return await Result.FailAsync("No sell order found.");
                }

                var sellOffers = _tokenService.GetNftSellOffers(nft.TokenId);
                var tokenOfferIds = sellOffers.Offers.Select(x => x.Index).ToList();

                var cancelSellOfferResult = _tokenService.CancelOffer(_context.UserAccountAddress, _context.UserAccountSecret, tokenOfferIds);
                if (!cancelSellOfferResult.Succeeded) return await Result.FailAsync(cancelSellOfferResult.Messages);

                await RemoveIndexSellOffersAsync(request.TokenId);

                return await Result.SuccessAsync();
            }

            private async Task RemoveIndexSellOffersAsync(string tokenId)
            {
                var idxSellOffers = await _dbContext.NFTSellOffers.AsQueryable().Where(x => x.TokenId == tokenId).ToListAsync();
                _dbContext.NFTSellOffers.RemoveRange(idxSellOffers);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
