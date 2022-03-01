using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configuration
{
    public class TransactionLogConfiguration : IEntityTypeConfiguration<TransactionLog>
    {
        public void Configure(EntityTypeBuilder<TransactionLog> builder)
        {
            builder.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.TxHash).HasMaxLength(256);
            builder.Property(p => p.Action).HasMaxLength(512).IsRequired();
        }
    }
}
