using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Transactions.EventHandlers
{
    public class TransactionLogAddedEventHandler : INotificationHandler<DomainEventNotification<TransactionLogAddEvent>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;

        public TransactionLogAddedEventHandler(IApplicationDbContext dbContext, IDateTime dateTime)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
        }


        public async Task Handle(DomainEventNotification<TransactionLogAddEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _dbContext.TransactionLogs.Add(new Domain.Entities.TransactionLog()
            {
                TxHash = domainEvent.TxHash,
                Action = domainEvent.Action,
                CreatorId = domainEvent.CreatorId,
                DateOccured = _dateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
