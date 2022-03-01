using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Feeds.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Feeds.Queries.GetRecentPosts
{
    public class GetRecentPostsQuery : IRequest<Result<List<FeedPostItemDto>>>
    {
        public class GetFeedPostsQueryHandler : IRequestHandler<GetRecentPostsQuery, Result<List<FeedPostItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;
            public GetFeedPostsQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _configuration = configuration;
            }

            public async Task<Result<List<FeedPostItemDto>>> Handle(GetRecentPostsQuery request, CancellationToken cancellationToken)
            {
                var posts = await _dbContext.FeedPostItems.AsQueryable().Where(x => x.CreatorIsAccountValid)
                    .OrderByDescending(x => x.PostDatePosted)
                    .Take(50)
                    .ToListAsync();

                var oldPostDate = posts.Select(x => x.PostDatePosted).Min();

                var likes = await _dbContext.FeedPostLikes.AsQueryable().Where(x => x.LikedById == _context.UserId && x.DateOccured > oldPostDate).ToListAsync();

                var data = new List<FeedPostItemDto>();

                foreach (var post in posts)
                {
                    var mappedPost = _mapper.Map<FeedPostItemDto>(post);

                    if (!string.IsNullOrEmpty(post.CreatorProfilePictureFilename))
                    {
                        mappedPost.CreatorProfilePictureLink = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.Creators}/{post.CreatorProfilePictureFilename}").OriginalString;
                    }

                    mappedPost.PostImageUri = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.FeedPost}/{post.PostFilename}").OriginalString;
                    mappedPost.IsLiked = likes.Any(x => x.PostId == post.PostId);

                    data.Add(mappedPost);
                }

                return await Result<List<FeedPostItemDto>>.SuccessAsync(data);
            }
        }
    }
}
