using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Action).HasMaxLength(512).IsRequired();
            builder.Property(p => p.WalletAddress).HasMaxLength(256);
            builder.Property(p => p.TxHash).HasMaxLength(256);

            builder.HasIndex(p => p.WalletAddress);
            builder.HasIndex(p => p.TxHash);
        }
    }
}
