using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class FeedPostConfiguration : IEntityTypeConfiguration<FeedPost>
    {
        public void Configure(EntityTypeBuilder<FeedPost> builder)
        {
            builder.HasOne(x => x.PostedBy)
                    .WithMany()
                    .HasForeignKey(x => x.PostedById)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Caption).HasMaxLength(140);
            builder.Property(p => p.Filename).HasMaxLength(256).IsRequired();
        }
    }
}
