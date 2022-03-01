using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Feeds.Commands.CreatePost
{
    public class CreateFeedPostCommand : IRequest<IResult>
    {
        public string Filename { get; set; }
        public string Caption { get; set; }

        public class FeedPostCommandHandler : IRequestHandler<CreateFeedPostCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IDateTime _dateTime;

            public FeedPostCommandHandler(ICallContext context, IApplicationDbContext dbContext, IAzureStorageBlobService blobService, IDateTime dateTime)
            {
                _context = context;
                _dbContext = dbContext;
                _blobService = blobService;
                _dateTime = dateTime;
            }


            public async Task<IResult> Handle(CreateFeedPostCommand request, CancellationToken cancellationToken)
            {
                var copyBlobResult = await _blobService.CopyAsync(BlobContainers.Dump, request.Filename, BlobContainers.FeedPost);
                if (!copyBlobResult.Succeeded) return await Result.FailAsync(copyBlobResult.Messages);

                var post = new FeedPost()
                {
                    PostedById = _context.UserId,
                    DatePosted = _dateTime.UtcNow,
                    Caption = request.Caption,
                    Filename = copyBlobResult.Data
                };

                _dbContext.FeedPosts.Add(post);
                await _dbContext.SaveChangesAsync();
                return await Result.SuccessAsync();
            }
        }
    }
}
