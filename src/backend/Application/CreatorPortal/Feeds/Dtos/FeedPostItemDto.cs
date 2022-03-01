using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.CreatorPortal.Feeds.Dtos
{
    public class FeedPostItemDto : IMapFrom<FeedPostItem>
    {
        public int PostId { get; set; }
        public string PostCaption { get; set; }
        public string PostImageUri { get; set; }
        public DateTime PostDatePosted { get; set; }

        public string CreatorUsername { get; set; }
        public string CreatorProfilePictureLink { get; set; }
        public int LikedCount { get; set; }
        public bool IsLiked { get; set; }
    }
}
