using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration
{
    public class NFTClaimConfiguration : IEntityTypeConfiguration<NFTClaim>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<NFTClaim> builder)
        {
            builder.Property(t => t.TokenId).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Uri).HasMaxLength(100).IsRequired();
            builder.Property(t => t.UriHex).HasMaxLength(512).IsRequired();
            builder.Property(t => t.SellOfferIndex).HasMaxLength(512).IsRequired();
            builder.Property(t => t.Message).HasMaxLength(512);

            builder.HasIndex(t => t.Uri);
            builder.HasIndex(t => t.UriHex);
            builder.HasIndex(t => t.TokenId);
            builder.HasIndex(t => t.SellOfferIndex);

        }
    }
}
