using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Feeds.Commands.CreatePost;
using Application.CreatorPortal.Feeds.Commands.DeletePost;
using Application.CreatorPortal.Feeds.Commands.LikePost;
using Application.CreatorPortal.Feeds.Dtos;
using Application.CreatorPortal.Feeds.Queries.GetMyPosts;
using Application.CreatorPortal.Feeds.Queries.GetRecentPosts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.CreatorPortal
{
    public class FeedController : HttpFunctionBase
    {
        public FeedController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("CreatorPortal_Feed_GetRecentPosts")]
        public async Task<IActionResult> GetRecentPosts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/feeds/recent")] GetRecentPostsQuery commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetRecentPostsQuery, Result<List<FeedPostItemDto>>>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Feed_GetPosts")]
        public async Task<IActionResult> GetPosts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/feeds/posts")] GetFeedPostsQuery commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetFeedPostsQuery, Result<List<FeedPostItemDto>>>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Feed_CreatePost")]
        public async Task<IActionResult> CreatePost([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/feeds/post")] CreateFeedPostCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<CreateFeedPostCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Feed_DeletePost")]
        public async Task<IActionResult> DeletePost([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/feeds/post/delete")] DeleteFeedPostCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<DeleteFeedPostCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Feed_LikePost")]
        public async Task<IActionResult> LikePost([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/feeds/post/like")] LikeFeedPostCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<LikeFeedPostCommand, Result<LikeFeedPostResponse>>(context, logger, req, commandArgs);
        }
    }
}
