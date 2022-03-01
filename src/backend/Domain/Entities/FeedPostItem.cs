using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FeedPostItem
    {
        public int PostId { get; set; }
        public string PostCaption { get; set; }
        public string PostFilename { get; set; }
        public DateTime PostDatePosted { get; set; }
        public int CreatorId { get; set; }
        public string CreatorUsername { get; set; }
        public string CreatorProfilePictureFilename { get; set; }
        public bool CreatorIsAccountValid { get; set; }
        public int LikedCount { get; set; }
    }
}
