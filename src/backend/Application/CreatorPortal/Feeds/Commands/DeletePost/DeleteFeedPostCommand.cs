using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Feeds.Commands.DeletePost
{
    public class DeleteFeedPostCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteFeedPostCommandHandler : IRequestHandler<DeleteFeedPostCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public DeleteFeedPostCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(DeleteFeedPostCommand request, CancellationToken cancellationToken)
            {
                var post = await _dbContext.FeedPosts.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Id && x.PostedById == _context.UserId);

                if (post == null) return await Result.FailAsync("Post not found.");

                var likes = await _dbContext.FeedPostLikes.AsQueryable().Where(x => x.PostId == request.Id).ToListAsync();
                _dbContext.FeedPostLikes.RemoveRange(likes);
                _dbContext.FeedPosts.Remove(post);
                await _dbContext.SaveChangesAsync();
                return await Result.SuccessAsync();
            }
        }
    }
}
