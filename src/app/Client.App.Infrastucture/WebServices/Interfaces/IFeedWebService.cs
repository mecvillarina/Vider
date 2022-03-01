using Application.Common.Models;
using Application.CreatorPortal.Feeds.Commands.CreatePost;
using Application.CreatorPortal.Feeds.Commands.DeletePost;
using Application.CreatorPortal.Feeds.Commands.LikePost;
using Application.CreatorPortal.Feeds.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IFeedWebService : IWebService
    {
        Task<IResult<List<FeedPostItemDto>>> GetRecentPostsAsync(string accessToken);
        Task<IResult<List<FeedPostItemDto>>> GetPostsAsync(int creatorId, string accessToken);
        Task<IResult> CreatePostAsync(CreateFeedPostCommand request, string accessToken);
        Task<IResult> DeletePostAsync(DeleteFeedPostCommand request, string accessToken);
        Task<IResult<LikeFeedPostResponse>> LikePostAsync(LikeFeedPostCommand request, string accessToken);
    }
}