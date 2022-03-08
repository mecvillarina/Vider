using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.SellNFT
{
    public class SellNFTCommand : IRequest<IResult>
    {
        public int Amount { get; set; }
        public bool IsExclusiveForSubscribers { get; set; }
        public int Id { get; set; }
        public string TokenId { get; set; }

        public class SellNFTCommandHandler : IRequestHandler<SellNFTCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDateTime _dateTime;
            private readonly IDomainEventService _domainEventService;

            public SellNFTCommandHandler(IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext, IDateTime dateTime, IDomainEventService domainEventService)
            {
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
                _dateTime = dateTime;
                _domainEventService = domainEventService;
            }

            public async Task<IResult> Handle(SellNFTCommand request, CancellationToken cancellationToken)
            {
                var nft = await _dbContext.NFTIndexes.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Id && x.TokenId == request.TokenId);

                if (nft == null) return await Result.FailAsync("No sellable NFT found.");

                var accountNftsResult = _tokenService.GetAccountNFTs(_context.UserAccountAddress);
                if (accountNftsResult.Status == "error") return await Result.FailAsync(accountNftsResult.ErrorMessage);

                if (!accountNftsResult.AccountNfts.Any(x => x.Uri == nft.UriHex && x.TokenId == nft.TokenId)) return await Result.FailAsync("No sellable NFT found.");

                var currentSellOffers = _tokenService.GetNftSellOffers(nft.TokenId);
                if (currentSellOffers.Offers != null)
                {
                    if (currentSellOffers.Offers.Any(x => x.Destination != null))
                    {
                        return await Result.FailAsync("Selected NFT has been gifted.");
                    }
                    else if (currentSellOffers.Offers.Any())
                    {
                        return await Result.FailAsync("Selected NFT is already for sale.");
                    }
                }

                var createSellOfferResult = _tokenService.CreateSellOffer(_context.UserAccountAddress, _context.UserAccountSecret, nft.TokenId, (request.Amount * AppConstants.DropPerXRP).ToString(), null);
                if (!createSellOfferResult.Succeeded) return await Result.FailAsync(createSellOfferResult.Messages);

                var sellOffers = _tokenService.GetNftSellOffers(nft.TokenId);
                var sellOffer = sellOffers.Offers.First();

                _dbContext.NFTSellOffers.Add(new NFTSellOffer()
                {
                    SellerId = _context.UserId,
                    TokenId = nft.TokenId,
                    TokenTaxon = nft.TokenTaxon,
                    Uri = nft.Uri,
                    UriHex = nft.UriHex,
                    SellOfferIndex = sellOffer.Index,
                    Amount = sellOffer.Amount,
                    DatePosted = _dateTime.UtcNow,
                    IsExclusiveForSubscribers = request.IsExclusiveForSubscribers
                });

                await _dbContext.SaveChangesAsync();

                var metadata = JsonConvert.DeserializeObject<NFTMetadata>(nft.Metadata);
                await _domainEventService.Publish(new ActivityLogAddEvent(_context.UserId, _context.UserAccountAddress, $"You've created a sell offer for your NFT ({metadata.Id}) for {request.Amount} XRP.", _dateTime.UtcNow, createSellOfferResult.Data));

                return await Result.SuccessAsync();
            }
        }
    }
}
