using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.ToView("SubscriberView")
                .HasKey("Id");

            //migrationBuilder.Sql(@"
            //CREATE VIEW SubscriberView AS
            //SELECT T2.Id, T1.Id as CreatorId, T2.SubscriberId, T3.Username as SubscriberUsername, T3.ProfilePictureFilename as SubscriberProfilePictureFilename , T3.IsAccountValid as SubscriberIsAccountValid, T2.DateSubscribed FROM Creators T1
            //JOIN CreatorSubscribers T2 ON T1.Id = T2.CreatorId
            //JOIN Creators T3 ON T2.SubscriberId = T3.Id
            //    ");

            //migrationBuilder.Sql(@"
            //    drop view SubscriberView;
            //");
        }
    }
}
