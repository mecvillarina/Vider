using Application.Common.Models;
using Application.CreatorPortal.Feeds.Commands.CreatePost;
using Application.CreatorPortal.Feeds.Commands.DeletePost;
using Application.CreatorPortal.Feeds.Commands.LikePost;
using Application.CreatorPortal.Feeds.Dtos;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class FeedManager : ManagerBase, IFeedManager
    {
        private readonly IFeedWebService _feedWebService;
        public FeedManager(IManagerToolkit managerToolkit, IFeedWebService feedWebService) : base(managerToolkit)
        {
            _feedWebService = feedWebService;
        }

        public async Task<IResult<List<FeedPostItemDto>>> GetRecentPostsAsync()
        {
            await PrepareForWebserviceCall();
            return await _feedWebService.GetRecentPostsAsync(AccessToken);
        }

        public async Task<IResult<List<FeedPostItemDto>>> GetPostsAsync(int creatorId = 0)
        {
            await PrepareForWebserviceCall();
            return await _feedWebService.GetPostsAsync(creatorId, AccessToken);
        }

        public async Task<IResult> CreatePostAsync(CreateFeedPostCommand request)
        {
            await PrepareForWebserviceCall();
            return await _feedWebService.CreatePostAsync(request, AccessToken);
        }

        public async Task<IResult> DeletePostAsync(DeleteFeedPostCommand request)
        {
            await PrepareForWebserviceCall();
            return await _feedWebService.DeletePostAsync(request, AccessToken);
        }

        public async Task<IResult<LikeFeedPostResponse>> LikePostAsync(LikeFeedPostCommand request)
        {
            await PrepareForWebserviceCall();
            return await _feedWebService.LikePostAsync(request, AccessToken);
        }

    }
}
