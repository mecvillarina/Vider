using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration
{
    public class CreatorSubscriberConfiguration : IEntityTypeConfiguration<CreatorSubscriber>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CreatorSubscriber> builder)
        {
            builder.HasOne(x => x.Creator)
                   .WithMany()
                   .HasForeignKey(x => x.CreatorId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Subscriber)
                  .WithMany()
                  .HasForeignKey(x => x.SubscriberId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.TxHash).HasMaxLength(256).IsRequired();

            builder.HasIndex(p => p.TxHash).IsUnique();
        }
    }
}
