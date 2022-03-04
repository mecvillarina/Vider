using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.NFTSellOffersCheck
{
    public class NFTSellOffersCheckCommand : IRequest<IResult>
    {
        public class SellOffersCheckCommandHandler : IRequestHandler<NFTSellOffersCheckCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageQueueService _queueService;
            public SellOffersCheckCommandHandler(IXrplNFTTokenService tokenService, IApplicationDbContext dbContext, IAzureStorageQueueService queueService)
            {
                _dbContext = dbContext;
                _queueService = queueService;
            }

            public async Task<IResult> Handle(NFTSellOffersCheckCommand request, CancellationToken cancellationToken)
            {
                var sellOffers = await _dbContext.NFTSellOfferItems.AsQueryable().ToListAsync();

                foreach (var sellOffer in sellOffers)
                {
                    _queueService.InsertMessage(QueueNames.CheckNFTSellerOffers, JsonConvert.SerializeObject(sellOffer));
                }

                return await Result.SuccessAsync();
            }
        }
    }
}
