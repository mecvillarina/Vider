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
    public class FeedPostItemConfiguration : IEntityTypeConfiguration<FeedPostItem>
    {
        public void Configure(EntityTypeBuilder<FeedPostItem> builder)
        {
            builder.ToView("FeedPostView")
                .HasKey(t => t.PostId);

            //migrationBuilder.Sql(@"
            //    CREATE VIEW FeedPostView as
            //SELECT T1.Id as PostId, T1.Caption as PostCaption, T1.[Filename] as PostFilename, T1.DatePosted as PostDatePosted, T2.Id as CreatorId, T2.Username as CreatorUsername, T2.ProfilePictureFilename as CreatorProfilePictureFilename, T2.IsAccountValid as CreatorIsAccountValid, COUNT(T3.Id) as LikedCount FROM FeedPosts T1
            //JOIN Creators T2 ON T1.PostedById = T2.Id
            //LEFT JOIN FeedPostLikes T3 ON T1.Id = T3.PostId
            //GROUP BY T1.Id, T1.Caption, T1.[Filename], T1.DatePosted, T2.Id, T2.Username, T2.ProfilePictureFilename, T2.IsAccountValid
            //    ");
            //migrationBuilder.Sql(@"
            //    drop view FeedPostView;
            //");
        }
    }
}
