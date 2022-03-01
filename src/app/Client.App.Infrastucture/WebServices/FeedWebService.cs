using Application.Common.Models;
using Application.CreatorPortal.Feeds.Commands.CreatePost;
using Application.CreatorPortal.Feeds.Commands.DeletePost;
using Application.CreatorPortal.Feeds.Commands.LikePost;
using Application.CreatorPortal.Feeds.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class FeedWebService : WebServiceBase, IFeedWebService
    {
        public FeedWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }
        public Task<IResult<List<FeedPostItemDto>>> GetRecentPostsAsync(string accessToken) => GetAsync<List<FeedPostItemDto>>(FeedEndpoints.GetRecentPosts, accessToken);
        public Task<IResult<List<FeedPostItemDto>>> GetPostsAsync(int creatorId, string accessToken) => GetAsync<List<FeedPostItemDto>>(string.Format(FeedEndpoints.GetPosts, creatorId), accessToken);
        public Task<IResult> CreatePostAsync(CreateFeedPostCommand request, string accessToken) => PostAsync(FeedEndpoints.CreatePost, request, accessToken);
        public Task<IResult> DeletePostAsync(DeleteFeedPostCommand request, string accessToken) => PostAsync(FeedEndpoints.DeletePost, request, accessToken);
        public Task<IResult<LikeFeedPostResponse>> LikePostAsync(LikeFeedPostCommand request, string accessToken) => PostAsync<LikeFeedPostCommand, LikeFeedPostResponse>(FeedEndpoints.LikePost, request, accessToken);
    }
}
