using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.CreatorSubscribers.Commands.Subscribe;
using Application.CreatorPortal.CreatorSubscribers.Queries.GetAll;
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
    public class CreatorSubscriberController : HttpFunctionBase
    {
        public CreatorSubscriberController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("CreatorPortal_Creator_Subscribers_Subscribe")]
        public async Task<IActionResult> Subscribe([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/creator/subscribers/subscribe")] SubscribeCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<SubscribeCommand, Result<int>>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Creator_Subscribers_GetSubscribers")]
        public async Task<IActionResult> GetSubscribers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/creator/subscribers")] GetAllSubscribersQuery commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetAllSubscribersQuery, Result<List<SubscriberDto>>>(context, logger, req, commandArgs);
        }
    }
}
