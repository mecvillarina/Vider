using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.CreatorRewards.Commands.Delete
{
    public class DeleteRewardCommand : IRequest<IResult>
    {
        public int RewardId { get; set; }

        public class DeleteRewardCommandHandler : IRequestHandler<DeleteRewardCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            public DeleteRewardCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(DeleteRewardCommand request, CancellationToken cancellationToken)
            {
                var reward = await _dbContext.CreatorRewards.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.RewardId);

                if (reward == null) throw new NotFoundException();

                if (reward.CreatorId != _context.UserId) return await Result.FailAsync();

                _dbContext.CreatorRewards.Remove(reward);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
