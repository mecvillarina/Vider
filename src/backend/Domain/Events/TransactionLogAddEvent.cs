using Domain.Common;
using System;

namespace Domain.Events
{
    public class TransactionLogAddEvent : DomainEvent
    {
        public string TxHash { get; private set; }
        public int CreatorId { get; private set; }
        public string Action { get; private set; }
        public DateTime DateOccured { get; private set; }
        public TransactionLogAddEvent(int creatorId, string action, DateTime dateOccured, string txHash = "")
        {
            TxHash = txHash;
            CreatorId = creatorId;
            Action = action;
            DateOccured = dateOccured;
        }
    }
}
