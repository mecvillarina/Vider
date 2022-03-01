using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.NFTs.Dtos;
using Domain.Enums;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Queries.GetCreatorNFTs
{
    public class GetCreatorNFTsQuery : IRequest<Result<List<NFTItemDto>>>
    {
        public int CreatorId { get; set; }

        public class GetCreatorNFTsQueryHandler : IRequestHandler<GetCreatorNFTsQuery, Result<List<NFTItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly ICreatorIdentityService _identityService;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            public GetCreatorNFTsQueryHandler(ICreatorIdentityService identityService, IXrplNFTTokenService tokenService, ICallContext context, IApplicationDbContext dbContext)
            {
                _identityService = identityService;
                _tokenService = tokenService;
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<Result<List<NFTItemDto>>> Handle(GetCreatorNFTsQuery request, CancellationToken cancellationToken)
            {
                request.CreatorId = request.CreatorId != 0 ? request.CreatorId : _context.UserId;

                var creator = await _identityService.GetAsync(request.CreatorId);

                var accountNftsResult = _tokenService.GetAccountNFTs(creator.AccountAddress);
                if (accountNftsResult.Status == "error") return await Result<List<NFTItemDto>>.FailAsync(accountNftsResult.ErrorMessage);

                var accountNfts = accountNftsResult.AccountNfts;
                var data = new List<NFTItemDto>();

                foreach (var accountNft in accountNfts)
                {
                    var uriHex = accountNft.Uri;
                    var uri = new string(accountNft.Uri.FromHexStringToString().Reverse().ToArray());
                    if (uri.Contains(BlobContainers.NFTMetadata))
                    {
                        var nft = _dbContext.NFTIndexes.AsQueryable().FirstOrDefault(x => x.UriHex == uriHex && x.TokenId == accountNft.TokenId);
                        var metadata = JsonConvert.DeserializeObject<NFTMetadata>(nft.Metadata);

                        var mintFlags = (NFTFlag)Enum.Parse(typeof(NFTFlag), accountNft.Flags.ToString());

                        var row = new NFTItemDto()
                        {
                            Id = nft.Id,
                            Metadata = metadata,
                            TokenId = accountNft.TokenId,
                            IsBurnable = (mintFlags & NFTFlag.Burnable) == NFTFlag.Burnable,
                            IsTransferable = (mintFlags & NFTFlag.Transferable) == NFTFlag.Transferable,
                            IsOnlyXRP = (mintFlags & NFTFlag.OnlyXRP) == NFTFlag.OnlyXRP
                        };

                        var currentSellOffers = _tokenService.GetNftSellOffers(accountNft.TokenId);

                        var giftOffer = currentSellOffers.Offers?.FirstOrDefault(x => !string.IsNullOrEmpty(x.Destination) && x.Destination != creator.AccountClassicAddress);

                        if (giftOffer == null)
                        {
                            var sellOffer = currentSellOffers.Offers?.FirstOrDefault(x => x.Destination != creator.AccountClassicAddress);

                            if (sellOffer != null)
                            {
                                var idxSellOffer = _dbContext.NFTSellOffers.AsQueryable().FirstOrDefault(x => x.SellOfferIndex == sellOffer.Index);

                                if (idxSellOffer != null)
                                {
                                    row.SellOffer = new NFTSellOfferDto()
                                    {
                                        Id = idxSellOffer.Id,
                                        IsExclusiveForSubscribers = idxSellOffer.IsExclusiveForSubscribers,
                                        Amount = Convert.ToDouble(idxSellOffer.Amount) / AppConstants.DropPerXRP,
                                    };
                                }
                            }

                            data.Add(row);
                        }
                    }
                }

                return await Result<List<NFTItemDto>>.SuccessAsync(data);
            }
        }
    }
}
