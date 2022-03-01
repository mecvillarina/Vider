using Application.Common.Models;
using Application.CreatorPortal.Feeds.Commands.CreatePost;
using Application.CreatorPortal.Feeds.Commands.DeletePost;
using Application.CreatorPortal.Feeds.Commands.LikePost;
using Application.CreatorPortal.Feeds.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IFeedManager : IManager
    {
        Task<IResult<List<FeedPostItemDto>>> GetRecentPostsAsync();
        Task<IResult<List<FeedPostItemDto>>> GetPostsAsync(int creatorId = 0);
        Task<IResult> CreatePostAsync(CreateFeedPostCommand request);
        Task<IResult> DeletePostAsync(DeleteFeedPostCommand request);
        Task<IResult<LikeFeedPostResponse>> LikePostAsync(LikeFeedPostCommand request);
    }
}