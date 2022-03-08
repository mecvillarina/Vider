using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.MintNFT
{
    public class MintNFTCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public bool IsBurnable { get; set; }
        public string Filename { get; set; }

        public class MintNFTCommandHandler : IRequestHandler<MintNFTCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IXrplNFTTokenService _tokenService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IConfiguration _configuration;
            private readonly IDateTime _dateTime;
            private readonly IDomainEventService _domainEventService;

            public MintNFTCommandHandler(ICallContext context, IXrplNFTTokenService tokenService, IApplicationDbContext dbContext, IAzureStorageBlobService blobService, IConfiguration configuration, IDateTime dateTime, IDomainEventService domainEventService)
            {
                _context = context;
                _tokenService = tokenService;
                _dbContext = dbContext;
                _blobService = blobService;
                _configuration = configuration;
                _dateTime = dateTime;
                _domainEventService = domainEventService;
            }

            public async Task<IResult> Handle(MintNFTCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.Filename))
                {
                    return await Result.FailAsync("Image is required.");
                }

                if (!AppConstants.AllowableNFTFormats.Split(",").Any(x => request.Filename.EndsWith(x.Trim())))
                {
                    return await Result.FailAsync("NFT format is not supported.");
                }

                var copyBlobResult = await _blobService.CopyAsync(BlobContainers.Dump, request.Filename, BlobContainers.NFT);
                if (!copyBlobResult.Succeeded) return await Result<int>.FailAsync(copyBlobResult.Messages);

                var metadata = new NFTMetadata()
                {
                    Id = Guid.NewGuid().ToString().Split('-').Last().ToUpper(),
                    Creator = _context.Username,
                    Name = request.Name,
                    Uri = $"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.NFT}/{copyBlobResult.Data}",
                    Created = _dateTime.UtcNowOffset
                };

                var mdFilename = $"{Guid.NewGuid()}.json";
                await _blobService.UploadAsync(metadata.ToStream(), BlobContainers.NFTMetadata, mdFilename);

                var mdUri = $"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.NFTMetadata}/{mdFilename}";
                var mdUriHexValue = mdUri.ToHexString();

                var mintFlags = NFTFlag.Transferable | NFTFlag.OnlyXRP;

                if (request.IsBurnable)
                {
                    mintFlags |= NFTFlag.Burnable;
                }

                var mintResult = _tokenService.Mint(_context.UserAccountAddress, _context.UserAccountSecret, mdUriHexValue, 0, (int)mintFlags);
                if (!mintResult.Succeeded) return await Result.FailAsync(mintResult.Messages);

                var accountNfts = _tokenService.GetAccountNFTs(_context.UserAccountAddress);
                var accountNft = accountNfts.AccountNfts.First(x => x.Uri == mdUriHexValue);

                var uriHex = accountNft.Uri;
                var uri = new string(uriHex.FromHexStringToString().Reverse().ToArray());
                _dbContext.NFTIndexes.Add(new NFTIndex()
                {
                    Metadata = JsonConvert.SerializeObject(metadata),
                    Created = metadata.Created,
                    Uri = uri,
                    UriHex = uriHex,
                    TokenId = accountNft.TokenId,
                    TokenTaxon = accountNft.TokenTaxon,
                    TokenFlags = accountNft.Flags,
                    NftSerial = accountNft.NftSerial,
                });

                await _dbContext.SaveChangesAsync();
                await _domainEventService.Publish(new ActivityLogAddEvent(_context.UserId, _context.UserAccountAddress, $"You've minted an NFT ({metadata.Id}).", _dateTime.UtcNow, mintResult.Data));

                return await Result.SuccessAsync();
            }
        }
    }
}
