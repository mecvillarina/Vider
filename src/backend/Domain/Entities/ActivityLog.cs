using System;

namespace Domain.Entities
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public virtual Creator Creator { get; set; }
        public string WalletAddress { get; set; }

        public DateTime DateOccured { get; set; }
        public string Action { get; set; }
        public string TxHash { get; set; }
    }
}
