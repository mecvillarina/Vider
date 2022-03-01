using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration
{
    public class NFTIndexConfiguration : IEntityTypeConfiguration<NFTIndex>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<NFTIndex> builder)
        {
            builder.Property(t => t.TokenId).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Uri).HasMaxLength(100).IsRequired();
            builder.Property(t => t.UriHex).HasMaxLength(512).IsRequired();
            builder.Property(t => t.Metadata).HasMaxLength(2048).IsRequired();

            builder.HasIndex(t => t.Uri).IsUnique();
            builder.HasIndex(t => t.UriHex).IsUnique();
            builder.HasIndex(t => t.TokenId).IsUnique();

        }
    }
}
