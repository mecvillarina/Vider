using System;

namespace Domain.Entities
{
    public class FeedPost
    {
        public int Id { get; set; }

        public int PostedById { get; set; }
        public virtual Creator PostedBy { get; set; }

        public string Caption { get; set; }
        public string Filename { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
