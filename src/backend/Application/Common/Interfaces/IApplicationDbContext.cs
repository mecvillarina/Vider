using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Creator> Creators { get; set; }
        DbSet<CreatorPassword> CreatorPasswords { get; set; }
        DbSet<CreatorSubscriber> CreatorSubscribers { get; set; }
        DbSet<CreatorProfile> CreatorProfiles { get; set; }
        DbSet<ActivityLog> ActivityLogs { get; set; }
        DbSet<CreatorReward> CreatorRewards { get; set; }
        DbSet<NFTIndex> NFTIndexes { get; set; }
        DbSet<NFTClaim> NFTClaims { get; set; }
        DbSet<NFTSellOffer> NFTSellOffers { get; set; }
        DbSet<NFTSellOfferItem> NFTSellOfferItems { get; set; }
        DbSet<FeedPost> FeedPosts { get; set; }
        DbSet<FeedPostLike> FeedPostLikes { get; set; }
        DbSet<FeedPostItem> FeedPostItems { get; set; }
        DbSet<Subscriber> Subscribers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}