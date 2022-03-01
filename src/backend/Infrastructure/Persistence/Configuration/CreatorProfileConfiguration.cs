using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CreatorProfileConfiguration : IEntityTypeConfiguration<CreatorProfile>
    {
        public void Configure(EntityTypeBuilder<CreatorProfile> builder)
        {
            builder.ToView("CreatorProfiles")
                .HasKey(t => t.Id);

            //migrationBuilder.Sql(@"
            //    CREATE VIEW CreatorProfiles as
            //    SELECT T1.Id, T1.Username, T1.UsernameNormalize, T1.Name, T1.Bio, T1.ProfilePictureFilename, T1.DateRegistered, COUNT(T2.Id) As SubscriberCount FROM Creators T1
            //    LEFT JOIN CreatorSubscribers T2 ON T1.Id = T2.CreatorId
            //    GROUP BY T1.Id,  T1.Username, T1.UsernameNormalize, T1.Name, T1.Bio,T1.ProfilePictureFilename, T1.DateRegistered");

            //migrationBuilder.Sql(@"
            //    drop view CreatorProfiles;
            //");
        }
    }
}
