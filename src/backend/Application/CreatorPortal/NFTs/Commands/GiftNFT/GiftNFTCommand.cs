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

namespace Application.CreatorPortal.NFTs.Commands.GiftNFT
{
    public class GiftNFTCommand : IRequest<IResult>
    {
        public string ReceiverUsername { get; set; }
        public int Id { get; set; }
        public string TokenId { get; set; }
        public string Message { get; set; }

        public class GiftNFTCommandHandler : IRequestHandler<GiftNFTCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly ICreatorIdentityService _identityService;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDomainEventService _domainEventService;
            private readonly IDateTime _dateTime;

            public GiftNFTCommandHandler(ICreatorIdentityService identityService, IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext, IDomainEventService domainEventService, IDateTime dateTime)
            {
                _identityService = identityService;
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
                _domainEventService = domainEventService;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(GiftNFTCommand request, CancellationToken cancellationToken)
            {
                var receiver = await _identityService.GetAsync(request.ReceiverUsername);

                if (receiver == null) return await Result.FailAsync("Receiver doesn't exist.");

                if (receiver.Username == _context.Username) return await Result.FailAsync("You cannot send gift to yourself.");

                var nft = await _dbContext.NFTIndexes.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Id && x.TokenId == request.TokenId);

                if (nft == null) return await Result.FailAsync("No giftable NFT found.");

                var accountNftsResult = _tokenService.GetAccountNFTs(_context.UserAccountAddress);
                if (accountNftsResult.Status == "error") return await Result.FailAsync(accountNftsResult.ErrorMessage);

                if (!accountNftsResult.AccountNfts.Any(x => x.Uri == nft.UriHex && x.TokenId == nft.TokenId)) return await Result.FailAsync("No giftable NFT found.");

                var currentSellOffers = _tokenService.GetNftSellOffers(nft.TokenId);
                if (currentSellOffers.Offers != null && currentSellOffers.Offers.Any()) return await Result.FailAsync("No giftable NFT found.");

                var createSellOfferResult = _tokenService.CreateSellOffer(_context.UserAccountAddress, _context.UserAccountSecret, nft.TokenId, "0", receiver.AccountClassicAddress);
                if (!createSellOfferResult.Succeeded) return await Result.FailAsync(createSellOfferResult.Messages);

                var sellOffers = _tokenService.GetNftSellOffers(nft.TokenId);
                var sellOffer = sellOffers.Offers.First(x => x.Destination == receiver.AccountClassicAddress);

                _dbContext.NFTClaims.Add(new NFTClaim()
                {
                    SenderId = _context.UserId,
                    ReceiverId = receiver.Id,
                    TokenId = nft.TokenId,
                    TokenTaxon = nft.TokenTaxon,
                    Uri = nft.Uri,
                    UriHex = nft.UriHex,
                    SellOfferIndex = sellOffer.Index,
                    Message = request.Message,
                });

                await _dbContext.SaveChangesAsync();

                var metadata = JsonConvert.DeserializeObject<NFTMetadata>(nft.Metadata);
                await _domainEventService.Publish(new ActivityLogAddEvent(_context.UserId, _context.UserAccountAddress, $"You've gifted {receiver.Username} an NFT ({metadata.Id}).", _dateTime.UtcNow, createSellOfferResult.Data));
                await _domainEventService.Publish(new ActivityLogAddEvent(receiver.Id, receiver.AccountAddress, $"{_context.Username} gifted you an NFT ({metadata.Id}). You can claim it anytime on your profile.", _dateTime.UtcNow, createSellOfferResult.Data));

                return await Result.SuccessAsync();
            }
        }
    }
}
