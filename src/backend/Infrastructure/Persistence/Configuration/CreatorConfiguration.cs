using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration
{
    public class CreatorConfiguration : IEntityTypeConfiguration<Creator>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Creator> builder)
        {
            builder.Property(t => t.Username).HasMaxLength(20).IsRequired();
            builder.Property(t => t.UsernameNormalize).HasMaxLength(20).IsRequired();
            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Bio).HasMaxLength(140);
            builder.Property(t => t.ProfilePictureFilename).HasMaxLength(100);

            builder.Property(p => p.Salt).HasMaxLength(256).IsRequired();

            builder.Property(p => p.AccountXAddress).HasMaxLength(256).IsRequired();
            builder.Property(p => p.AccountSecret).HasMaxLength(256).IsRequired();
            builder.Property(p => p.AccountClassicAddress).HasMaxLength(256).IsRequired();
            builder.Property(p => p.AccountAddress).HasMaxLength(256).IsRequired();

            builder.HasIndex(t => t.Username).IsUnique();
            builder.HasIndex(t => t.UsernameNormalize).IsUnique();
        }
    }
}
