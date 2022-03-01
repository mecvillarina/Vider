using System;

namespace Domain.Entities
{
    public class CreatorSubscriber
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }
        public virtual Creator Creator { get; set; }

        public int? SubscriberId { get; set; }
        public virtual Creator Subscriber { get; set; }

        public string TxHash { get; set; }

        public DateTime DateSubscribed { get; set; }
    }
}
