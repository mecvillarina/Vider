using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.NFTs.Dtos;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Queries.GetNFTSellOffers
{
    public class GetNFTSellOffersQuery : IRequest<Result<List<NFTSellOfferItemDto>>>
    {
        public string Query { get; set; }

        public class GetNFTSellOffersQueryHandler : IRequestHandler<GetNFTSellOffersQuery, Result<List<NFTSellOfferItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly ICreatorIdentityService _identityService;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;

            public GetNFTSellOffersQueryHandler(ICreatorIdentityService identityService, IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration)
            {
                _identityService = identityService;
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _configuration = configuration;
            }

            public async Task<Result<List<NFTSellOfferItemDto>>> Handle(GetNFTSellOffersQuery request, CancellationToken cancellationToken)
            {
                var subscriptionIds = await _dbContext.CreatorSubscribers.Where(x => x.SubscriberId == _context.UserId).Select(x => x.CreatorId).ToListAsync();

                var sellOffers = await _dbContext.NFTSellOfferItems.AsQueryable()
                    .Where(x => x.SellerAccountValid &&
                            //(x.SellerId != _context.UserId) &&
                            (x.SellerId == _context.UserId || !x.IsExclusiveForSubscribers || subscriptionIds.Any(y => y == x.SellerId)) &&
                            (string.IsNullOrEmpty(request.Query) || x.SellerUsername.Contains(request.Query.ToLower()) || x.NFTMetadata.Contains(request.Query)))
                    .OrderByDescending(x => x.DatePosted)
                    .Take(100)
                    .ToListAsync();

                var data = new List<NFTSellOfferItemDto>();

                foreach (var sellOffer in sellOffers)
                {
                    var mappedSellOffer = _mapper.Map<NFTSellOfferItemDto>(sellOffer);
                    if (!string.IsNullOrEmpty(sellOffer.SellerProfilePictureFilename))
                    {
                        mappedSellOffer.SellerProfilePictureLink = new Uri($"{_configuration["AzureStorageCdn"]}/{BlobContainers.Creators}/{sellOffer.SellerProfilePictureFilename}").OriginalString;
                    }

                    mappedSellOffer.SellerProfilePictureLink ??= "assets/default_photo.jpg";
                    mappedSellOffer.SellOfferId = sellOffer.SellOfferId;
                    mappedSellOffer.SellOfferDatePosted = sellOffer.DatePosted;
                    mappedSellOffer.SellOfferIsExclusiveForSubscribers = sellOffer.IsExclusiveForSubscribers;
                    mappedSellOffer.SellOfferAmount = Convert.ToDouble(sellOffer.Amount) / AppConstants.DropPerXRP;

                    var nftFlag = (NFTFlag)Enum.Parse(typeof(NFTFlag), sellOffer.NFTTokenFlags.ToString());

                    mappedSellOffer.NFT = new NFTDto()
                    {
                        TokenId = sellOffer.NFTTokenId,
                        IsBurnable = (nftFlag & NFTFlag.Burnable) == NFTFlag.Burnable,
                        IsTransferable = (nftFlag & NFTFlag.Transferable) == NFTFlag.Transferable,
                        IsOnlyXRP = (nftFlag & NFTFlag.OnlyXRP) == NFTFlag.OnlyXRP,
                        Metadata = JsonConvert.DeserializeObject<NFTMetadata>(sellOffer.NFTMetadata)
                    };

                    mappedSellOffer.CanBuy = _context.UserId != sellOffer.SellerId;
                    data.Add(mappedSellOffer);
                }

                return await Result<List<NFTSellOfferItemDto>>.SuccessAsync(data);
            }
        }
    }
}
