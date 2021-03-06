using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.BuyNFT
{
    public class BuyNFTCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string TokenId { get; set; }

        public class BuyNFTCommandHandler : IRequestHandler<BuyNFTCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IXrplAccountService _accountService;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDomainEventService _domainEventService;
            private readonly IDateTime _dateTime;

            public BuyNFTCommandHandler(ICallContext context, IXrplAccountService accountService, IXrplNFTTokenService tokenService, IApplicationDbContext dbContext, IDomainEventService domainEventService, IDateTime dateTime)
            {
                _context = context;
                _accountService = accountService;
                _tokenService = tokenService;
                _dbContext = dbContext;
                _domainEventService = domainEventService;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(BuyNFTCommand request, CancellationToken cancellationToken)
            {
                var sellOfferItem = await _dbContext.NFTSellOfferItems.AsQueryable().FirstOrDefaultAsync(x => x.SellOfferId == request.Id && x.NFTTokenId == request.TokenId);

                if (sellOfferItem == null) return await Result.FailAsync("No sell offer found. Probably the creator cancelled the sale or someone already bought it.");

                var currentSellOffers = _tokenService.GetNftSellOffers(sellOfferItem.NFTTokenId);
                if (currentSellOffers.Offers == null || !currentSellOffers.Offers.Any(x => x.Index == sellOfferItem.SellOfferIndex))
                {
                    await RemoveSaleOffer(sellOfferItem.SellOfferId);
                    return await Result.FailAsync("No sell offer found. Probably the creator cancelled the sale or someone already bought it.");
                }

                var acountInfo = _accountService.AccountInfo(_context.UserAccountAddress);
                if (acountInfo.Status == "error") return await Result<int>.FailAsync($"There's a problem on your wallet: {acountInfo.ErrorMessage}");

                var currentSellOffer = currentSellOffers.Offers.First(x => x.Index == sellOfferItem.SellOfferIndex);

                var walletBalance = Convert.ToDouble(acountInfo.AccountData.Balance);
                var sellOfferAmount = Convert.ToDouble(currentSellOffer.Amount);

                if (sellOfferAmount > walletBalance) return await Result.FailAsync("Not enough wallet balance.");

                var buySellOfferResult = _tokenService.AcceptSellOffer(_context.UserAccountAddress, _context.UserAccountSecret, currentSellOffer.Index);
                if (!buySellOfferResult.Succeeded) return await Result.FailAsync(buySellOfferResult.Messages);

                await RemoveSaleOffer(sellOfferItem.SellOfferId);

                var metadata = JsonConvert.DeserializeObject<NFTMetadata>(sellOfferItem.NFTMetadata);

                await _domainEventService.Publish(new ActivityLogAddEvent(_context.UserId, _context.UserAccountAddress, $"You've accepted {sellOfferItem.SellerUsername}'s NFT ({metadata.Id}) sell offer and bought it for {Convert.ToDouble(sellOfferItem.Amount) / AppConstants.DropPerXRP} XRP.", _dateTime.UtcNow, buySellOfferResult.Data));
                await _domainEventService.Publish(new ActivityLogAddEvent(sellOfferItem.SellerId, sellOfferItem.SellerAccountAddress, $"{_context.Username} accepted your sell offer and bought your NFT ({metadata.Id}) for {Convert.ToDouble(sellOfferItem.Amount) / AppConstants.DropPerXRP} XRP.", _dateTime.UtcNow, buySellOfferResult.Data));

                return await Result.SuccessAsync();
            }

            private async Task RemoveSaleOffer(int id)
            {
                var sellOffer = await _dbContext.NFTSellOffers.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

                if (sellOffer != null)
                {
                    _dbContext.NFTSellOffers.Remove(sellOffer);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
