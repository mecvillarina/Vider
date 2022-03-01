using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration
{
    public class NFTSellOfferConfiguration : IEntityTypeConfiguration<NFTSellOffer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<NFTSellOffer> builder)
        {
            builder.Property(t => t.TokenId).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Uri).HasMaxLength(100).IsRequired();
            builder.Property(t => t.UriHex).HasMaxLength(512).IsRequired();
            builder.Property(t => t.SellOfferIndex).HasMaxLength(512).IsRequired();

            builder.HasIndex(t => t.Uri);
            builder.HasIndex(t => t.UriHex);
            builder.HasIndex(t => t.TokenId);
            builder.HasIndex(t => t.SellOfferIndex);
        }
    }
}
