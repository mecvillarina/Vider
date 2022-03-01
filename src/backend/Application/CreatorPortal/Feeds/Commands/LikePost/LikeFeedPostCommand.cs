using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Feeds.Commands.LikePost
{
    public class LikeFeedPostCommand : IRequest<Result<LikeFeedPostResponse>>
    {
        public int PostId { get; set; }

        public class LikeFeedPostCommandHandler : IRequestHandler<LikeFeedPostCommand, Result<LikeFeedPostResponse>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDateTime _dateTime;
            public LikeFeedPostCommandHandler(ICallContext context, IApplicationDbContext dbContext, IDateTime dateTime)
            {
                _context = context;
                _dbContext = dbContext;
                _dateTime = dateTime;
            }

            public async Task<Result<LikeFeedPostResponse>> Handle(LikeFeedPostCommand request, CancellationToken cancellationToken)
            {
                var isPostExists = await _dbContext.FeedPosts.AsQueryable().AnyAsync(x => x.Id == request.PostId);

                if (!isPostExists) return await Result<LikeFeedPostResponse>.FailAsync("Post not found.");

                var postLike = await _dbContext.FeedPostLikes.AsQueryable().FirstOrDefaultAsync(x => x.LikedById == _context.UserId && x.PostId == request.PostId);

                if (postLike != null)
                {
                    _dbContext.FeedPostLikes.Remove(postLike);
                }
                else
                {
                    _dbContext.FeedPostLikes.Add(new FeedPostLike() { PostId = request.PostId, LikedById = _context.UserId, DateOccured = _dateTime.UtcNow });
                }

                await _dbContext.SaveChangesAsync();
                var count = await _dbContext.FeedPostLikes.CountAsync(x => x.PostId == request.PostId);
                var postLikeExists = await _dbContext.FeedPostLikes.AnyAsync(x => x.LikedById == _context.UserId && x.PostId == request.PostId);

                return await Result<LikeFeedPostResponse>.SuccessAsync(new LikeFeedPostResponse()
                {
                    IsLiked = postLikeExists,
                    Count = count
                });
            }
        }
    }
}
