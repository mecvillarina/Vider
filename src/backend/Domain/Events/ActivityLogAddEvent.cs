using Domain.Common;
using System;

namespace Domain.Events
{
    public class ActivityLogAddEvent : DomainEvent
    {
        public int CreatorId { get; private set; }
        public string WalletAddress { get; private set; }
        public DateTime DateOccured { get; private set; }
        public string Action { get; private set; }
        public string TxHash { get; set; }

        public ActivityLogAddEvent(int creatorId, string walletAddress, string action, DateTime dateOccured, string txHash)
        {
            CreatorId = creatorId;
            WalletAddress = walletAddress;
            Action = action;
            DateOccured = dateOccured;
            TxHash = txHash;
        }
    }
}
