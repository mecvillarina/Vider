using System;

namespace Domain.Entities
{
    public class FeedPostLike
    {
        public int Id { get; set; }

        public int? PostId { get; set; }
        public virtual FeedPost Post { get; set; }

        public int? LikedById { get; set; }
        public virtual Creator LikedBy { get; set; }
        public DateTime DateOccured { get; set; }
    }
}
