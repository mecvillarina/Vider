using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICallContext _context;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions options, IDomainEventService domainEventService, ICallContext context, IDateTime dateTime, IConfiguration configuration) : base(options)
        {
            _domainEventService = domainEventService;
            _context = context;
            _dateTime = dateTime;
            _configuration = configuration;
        }

        public DbSet<Creator> Creators { get; set; }
        public DbSet<CreatorPassword> CreatorPasswords { get; set; }
        public DbSet<CreatorSubscriber> CreatorSubscribers { get; set; }
        public DbSet<CreatorProfile> CreatorProfiles { get; set; }
        public DbSet<CreatorReward> CreatorRewards { get; set; }
        public DbSet<NFTIndex> NFTIndexes { get; set; }
        public DbSet<NFTClaim> NFTClaims { get; set; }
        public DbSet<NFTSellOffer> NFTSellOffers { get; set; }
        public DbSet<NFTSellOfferItem> NFTSellOfferItems { get; set; }
        public DbSet<FeedPost> FeedPosts { get; set; }
        public DbSet<FeedPostLike> FeedPostLikes { get; set; }
        public DbSet<FeedPostItem> FeedPostItems { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                var now = _dateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = now;
                        entry.Entity.LastModifiedDate = now;
                        entry.Entity.Partition = _configuration.GetValue<string>("PartitionKey");
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchEvents();
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                        .Select(x => x.Entity.DomainEvents)
                        .SelectMany(x => x)
                        .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}