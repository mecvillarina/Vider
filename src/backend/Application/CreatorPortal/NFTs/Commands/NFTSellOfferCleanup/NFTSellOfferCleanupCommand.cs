using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.NFTSellOfferCleanup
{
    public class NFTSellOfferCleanupCommand : IRequest<IResult>
    {
        public NFTSellOfferItem Message { get; set; }

        public class SellOfferCleanupCommandHandler : IRequestHandler<NFTSellOfferCleanupCommand, IResult>
        {
            private readonly IXrplAccountService _accountService;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;

            public SellOfferCleanupCommandHandler(IXrplAccountService accountService, IXrplNFTTokenService tokenService, IApplicationDbContext dbContext)
            {
                _accountService = accountService;
                _tokenService = tokenService;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(NFTSellOfferCleanupCommand request, CancellationToken cancellationToken)
            {
                var sellOfferItem = await _dbContext.NFTSellOfferItems.AsQueryable().FirstOrDefaultAsync(x => x.SellOfferId == request.Message.SellOfferId && x.NFTTokenId == request.Message.NFTTokenId);
                if (sellOfferItem == null) return await Result.FailAsync("No sell offer found. Probably the creator cancelled the sale or someone already bought it.");

                var currentSellOffers = _tokenService.GetNftSellOffers(sellOfferItem.NFTTokenId);
                if (currentSellOffers.Offers == null || !currentSellOffers.Offers.Any(x => x.Index == sellOfferItem.SellOfferIndex))
                {
                    await RemoveSaleOffer(sellOfferItem.SellOfferId);
                    return await Result.FailAsync("No sell offer found. Probably the creator cancelled the sale or someone already bought it.");
                }

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
