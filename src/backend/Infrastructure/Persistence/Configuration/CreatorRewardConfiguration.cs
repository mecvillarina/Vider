using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CreatorRewardConfiguration : IEntityTypeConfiguration<CreatorReward>
    {
        public void Configure(EntityTypeBuilder<CreatorReward> builder)
        {
            builder.HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(t => t.Filename).HasMaxLength(100);
            builder.Property(t => t.Name).HasMaxLength(100);
            builder.Property(t => t.Message).HasMaxLength(512);
            builder.Property(t => t.Taxon).IsRequired();

            builder.HasIndex(t => t.Taxon).IsUnique();
        }
    }
}
