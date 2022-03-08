using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Activities.EventHandlers
{
    public class ActivityLogAddedEventHandler : INotificationHandler<DomainEventNotification<ActivityLogAddEvent>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;

        public ActivityLogAddedEventHandler(IApplicationDbContext dbContext, IDateTime dateTime)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
        }


        public async Task Handle(DomainEventNotification<ActivityLogAddEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _dbContext.ActivityLogs.Add(new ActivityLog()
            {
                WalletAddress = domainEvent.WalletAddress,
                Action = domainEvent.Action,
                CreatorId = domainEvent.CreatorId,
                DateOccured = _dateTime.UtcNow,
                TxHash = domainEvent.TxHash,
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
