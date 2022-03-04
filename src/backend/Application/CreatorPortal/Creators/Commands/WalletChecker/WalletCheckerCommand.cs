using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Creators.Commands.WalletChecker
{
    public class WalletCheckerCommand : IRequest<IResult>
    {
        public class WalletCheckerCommandHandler : IRequestHandler<WalletCheckerCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageQueueService _queueService;
            public WalletCheckerCommandHandler(IApplicationDbContext dbContext, IAzureStorageQueueService queueService)
            {
                _dbContext = dbContext;
                _queueService = queueService;
            }

            public async Task<IResult> Handle(WalletCheckerCommand request, CancellationToken cancellationToken)
            {
                var creators = await _dbContext.Creators.AsQueryable().ToListAsync();

                foreach (var creator in creators)
                {
                    var queueMessage = new WalletCheckerQueueMessage()
                    {
                        CreatorId = creator.Id,
                    };

                    _queueService.InsertMessage(QueueNames.WalletChecker, JsonConvert.SerializeObject(queueMessage));
                }

                return await Result.SuccessAsync();
            }
        }
    }
}
