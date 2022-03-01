using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.CreatorRewards.Commands.Delete;
using Application.CreatorPortal.CreatorRewards.Commands.Upload;
using Application.CreatorPortal.CreatorRewards.Queries.GetRewards;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.CreatorPortal
{
    public class CreatorRewardController : HttpFunctionBase
    {
        public CreatorRewardController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("CreatorPortal_Creator_Rewards_GetAll")]
        public async Task<IActionResult> GetRewards([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/creator/rewards")] GetRewardsQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetRewardsQuery, Result<List<GetRewardItemDto>>>(context, logger, req, queryArgs);
        }

        [FunctionName("CreatorPortal_Creator_Rewards_Upload")]
        public async Task<IActionResult> UploadRewards([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/creator/reward")] UploadRewardCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);

            commandArgs ??= new UploadRewardCommand();

            var jsonData = JsonConvert.DeserializeObject<UploadRewardCommand>(req.Form["JsonPayload"].ToString());
            commandArgs.Name = jsonData.Name;
            commandArgs.Message = jsonData.Message;

            string name = $"AttachedFile";
            var file = req.Form.Files.GetFile(name);

            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();
            string extension = Path.GetExtension(filename);

            commandArgs.FileStream = stream;
            commandArgs.FileExtension = extension;

            return await ExecuteAsync<UploadRewardCommand, IResult>(context, logger, req, commandArgs);
        }

   
        [FunctionName("CreatorPortal_Creator_Rewards_Delete")]
        public async Task<IActionResult> DeleteReward([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/creator/rewards/delete")] DeleteRewardCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<DeleteRewardCommand, IResult>(context, logger, req, commandArgs);
        }
    }
}
