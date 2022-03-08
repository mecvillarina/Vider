using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.CreatorSubscribers.Commands.Subscribe
{
    public class SubscribeCommand : IRequest<Result<int>>
    {
        public int CreatorId { get; set; }

        public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, Result<int>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly ICreatorIdentityService _identityService;
            private readonly IDateTime _dateTime;
            private readonly IXrplAccountService _accountService;
            private readonly IXrplPaymentService _paymentService;
            private readonly IDomainEventService _domainEventService;
            private readonly IAzureStorageQueueService _queueService;
            public SubscribeCommandHandler(ICallContext context, IApplicationDbContext dbContext, ICreatorIdentityService identityService, IDateTime dateTime, IXrplAccountService accountService, IXrplPaymentService paymentService, IDomainEventService domainEventService, IMediator mediator, IAzureStorageQueueService queueService)
            {
                _context = context;
                _dbContext = dbContext;
                _identityService = identityService;
                _dateTime = dateTime;
                _accountService = accountService;
                _paymentService = paymentService;
                _domainEventService = domainEventService;
                _queueService = queueService;
            }

            public async Task<Result<int>> Handle(SubscribeCommand request, CancellationToken cancellationToken)
            {
                var subscriberId = _context.UserId;

                if (subscriberId == request.CreatorId) return await Result<int>.FailAsync("Subscribing to yourself is not allowed.");

                var admin = await _identityService.GetAsync("admin");
                if (admin == null) return await Result<int>.FailAsync("Admin is not exists.");

                var creator = await _identityService.GetAsync(request.CreatorId);
                if (creator == null) return await Result<int>.FailAsync("Creator is not exists.");

                var subscribe = await _dbContext.CreatorSubscribers.AsQueryable().FirstOrDefaultAsync(x => x.CreatorId == request.CreatorId && x.SubscriberId == subscriberId);
                if (subscribe != null) return await Result<int>.FailAsync($"You have already subscribed with {creator.Name}");

                var creatorAccountInfo = _accountService.AccountInfo(creator.AccountAddress);
                if (creatorAccountInfo.Status == "error") return await Result<int>.FailAsync($"There's a problem on creator wallet: {creatorAccountInfo.ErrorMessage}");

                var subscriberAccountInfo = _accountService.AccountInfo(_context.UserAccountAddress);
                if (subscriberAccountInfo.Status == "error") return await Result<int>.FailAsync($"There's a problem on your wallet: {subscriberAccountInfo.ErrorMessage}");

                var adminAccountInfo = _accountService.AccountInfo(admin.AccountAddress);
                if (adminAccountInfo.Status == "error") return await Result<int>.FailAsync($"There's a problem on platform wallet: {adminAccountInfo.ErrorMessage}");

                var platformPaymentResult = _paymentService.Pay(_context.UserAccountAddress, _context.UserAccountSecret, (AppConstants.SubscriptionCostInXRP * 0.01 * AppConstants.DropPerXRP).ToString(), admin.AccountAddress);
                if (!platformPaymentResult.Succeeded) return await Result<int>.FailAsync(platformPaymentResult.Messages);

                var creatorPaymentResult = _paymentService.Pay(_context.UserAccountAddress, _context.UserAccountSecret, (AppConstants.SubscriptionCostInXRP * 0.99 * AppConstants.DropPerXRP).ToString(), creator.AccountAddress);
                if (!creatorPaymentResult.Succeeded) return await Result<int>.FailAsync(creatorPaymentResult.Messages);

                var dateNow = _dateTime.UtcNow;
                var newSub = new CreatorSubscriber()
                {
                    CreatorId = request.CreatorId,
                    SubscriberId = subscriberId,
                    TxHash = creatorPaymentResult.Data,
                    DateSubscribed = dateNow
                };

                _dbContext.CreatorSubscribers.Add(newSub);
                await _dbContext.SaveChangesAsync();

                await _domainEventService.Publish(new ActivityLogAddEvent(creator.Id, creator.AccountAddress, $"You've received {AppConstants.SubscriptionCostInXRP * 0.99} XRP for new subscription from {_context.Username}.", dateNow, creatorPaymentResult.Data));
                await _domainEventService.Publish(new ActivityLogAddEvent(_context.UserId, _context.UserAccountAddress, $"You've successfully subscribed to {creator.Username}. {AppConstants.SubscriptionCostInXRP * 0.99} XRP (subscription fee) + {AppConstants.SubscriptionCostInXRP * 0.01} XRP (platform fee) + transaction fee in XRP has been deducted from your wallet. ", dateNow, creatorPaymentResult.Data));

                _queueService.InsertMessage(QueueNames.MintNFTSubscribeReward, JsonConvert.SerializeObject(new MintNFTSubscribeRewardQueueMessage() { CreatorId = creator.Id, SubscriberId = _context.UserId }));
                var subscriberCount = await _dbContext.CreatorSubscribers.AsQueryable().CountAsync(x => x.CreatorId == creator.Id);
                return await Result<int>.SuccessAsync(subscriberCount);
            }
        }
    }
}
