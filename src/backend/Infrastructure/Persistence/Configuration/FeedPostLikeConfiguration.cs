using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class FeedPostLikeConfiguration : IEntityTypeConfiguration<FeedPostLike>
    {
        public void Configure(EntityTypeBuilder<FeedPostLike> builder)
        {
            
        }
    }
}
