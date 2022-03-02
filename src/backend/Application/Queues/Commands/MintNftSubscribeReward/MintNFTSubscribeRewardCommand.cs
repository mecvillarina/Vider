using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queues.Commands.MintNftSubscribeReward
{
    public class MintNFTSubscribeRewardCommand : IRequest<IResult>
    {
        public MintNFTSubscribeRewardQueueMessage Message { get; set; }

        public class MintNFTSubscribeRewardCommandHandler : IRequestHandler<MintNFTSubscribeRewardCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly ICreatorIdentityService _identityService;
            private readonly IXrplAccountService _accountService;
            private readonly IXrplNFTTokenService _nftTokenService;
            private readonly IDomainEventService _domainEventService;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IConfiguration _configuration;
            private readonly IDateTime _dateTime;
            public MintNFTSubscribeRewardCommandHandler(IXrplAccountService accountService, IDomainEventService domainEventService, IApplicationDbContext dbContext, ICreatorIdentityService identityService, IXrplNFTTokenService nftTokenService, IAzureStorageBlobService blobService, IConfiguration configuration, IDateTime dateTime)
            {
                _accountService = accountService;
                _dbContext = dbContext;
                _domainEventService = domainEventService;
                _identityService = identityService;
                _nftTokenService = nftTokenService;
                _blobService = blobService;
                _configuration = configuration;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(MintNFTSubscribeRewardCommand request, CancellationToken cancellationToken)
            {
                int creatorId = request.Message.CreatorId;
                int subscriberId = request.Message.SubscriberId;

                var rewards = await _dbContext.CreatorRewards.Where(x => x.CreatorId == creatorId).ToListAsync();
                if (!rewards.Any()) return await Result<int>.FailAsync();

                var creator = await _identityService.GetAsync(creatorId);
                if (creator == null) return await Result<int>.FailAsync("Creator is not exists.");

                var subscriber = await _identityService.GetAsync(subscriberId);
                if (subscriber == null) return await Result<int>.FailAsync("Subscriber is not exists.");

                var creatorAccountInfo = _accountService.AccountInfo(creator.AccountAddress);

                if (creatorAccountInfo.Status == "error") return await Result<int>.FailAsync($"There's a problem on creator wallet: {creatorAccountInfo.ErrorMessage}");

                var subscriberAccountInfo = _accountService.AccountInfo(subscriber.AccountAddress);
                if (subscriberAccountInfo.Status == "error") return await Result<int>.FailAsync($"There's a problem on subscriber wallet: {subscriberAccountInfo.ErrorMessage}");

                var rewardIdx = new Random().Next(0, rewards.Count);
                var reward = rewards[rewardIdx];

                var copyBlobResult = await _blobService.CopyAsync(BlobContainers.CreatorRewards, reward.Filename, BlobContainers.NFT);
                if (!copyBlobResult.Succeeded) return await Result<int>.FailAsync(copyBlobResult.Messages);

                var metadata = new NFTMetadata()
                {
                    Id = Guid.NewGuid().ToString().Split('-').Last().ToUpper(),
                    Creator = creator.Username,
                    Name = reward.Name,
                    Uri = $"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.NFT}/{copyBlobResult.Data}",
                    Created = _dateTime.UtcNowOffset
                };

                var mdFilename = $"{Guid.NewGuid()}.json";
                await _blobService.UploadAsync(metadata.ToStream(), BlobContainers.NFTMetadata, mdFilename);

                var mdUri = $"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.NFTMetadata}/{mdFilename}";
                var mdUriHexValue = mdUri.ToHexString();

                var creatorAccountSecret = AESExtensions.Decrypt(creator.AccountSecret, creator.Salt);

                var mintResult = _nftTokenService.Mint(creator.AccountAddress, creatorAccountSecret, mdUriHexValue, reward.Taxon, 11);
                if (!mintResult.Succeeded) return await Result.FailAsync(mintResult.Messages);

                var accountNfts = _nftTokenService.GetAccountNFTs(creator.AccountAddress);
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

                var createSellOfferResult = _nftTokenService.CreateSellOffer(creator.AccountAddress, creatorAccountSecret, accountNft.TokenId, "0", subscriber.AccountClassicAddress);
                if (!createSellOfferResult.Succeeded) return await Result.FailAsync(createSellOfferResult.Messages);

                var sellOffers = _nftTokenService.GetNftSellOffers(accountNft.TokenId);
                var sellOffer = sellOffers.Offers.First(x => x.Destination == subscriber.AccountAddress);

                _dbContext.NFTClaims.Add(new NFTClaim()
                {
                    SenderId = creatorId,
                    ReceiverId = subscriberId,
                    TokenId = accountNft.TokenId,
                    TokenTaxon = accountNft.TokenTaxon,
                    Uri = new string(accountNft.Uri.FromHexStringToString().Reverse().ToArray()),
                    UriHex = accountNft.Uri,
                    SellOfferIndex = sellOffer.Index,
                    Message = reward.Message,
                });

                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
