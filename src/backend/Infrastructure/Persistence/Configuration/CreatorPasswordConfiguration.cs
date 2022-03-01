using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration
{
    public class CreatorPasswordConfiguration : IEntityTypeConfiguration<CreatorPassword>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CreatorPassword> builder)
        {
            builder.HasOne(x => x.Creator)
                     .WithMany()
                     .HasForeignKey(x => x.CreatorId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Salt).HasMaxLength(256).IsRequired();

            builder.Property(p => p.Digest).HasMaxLength(256).IsRequired();
        }
    }
}
