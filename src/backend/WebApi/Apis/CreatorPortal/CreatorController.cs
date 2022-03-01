using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Creators.Commands.UploadDump;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.Creators.Queries.Get;
using Application.CreatorPortal.Creators.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.CreatorPortal
{
    public class CreatorController : HttpFunctionBase
    {
        public CreatorController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("CreatorPortal_Creator_GetProfile")]
        public async Task<IActionResult> GetProfile([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/creator")] GetCreatorQuery commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetCreatorQuery, Result<CreatorDto>>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Creator_GetAll")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/creator/search")] GetAllCreatorQuery commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetAllCreatorQuery, Result<List<CreatorDto>>>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Creator_UploadDump")]
        public async Task<IActionResult> UploadDumpReward([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/creator/uploaddump")] UploadDumpCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);

            commandArgs ??= new UploadDumpCommand();

            string name = $"AttachedFile";
            var file = req.Form.Files.GetFile(name);

            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();
            string extension = Path.GetExtension(filename);

            commandArgs.FileStream = stream;
            commandArgs.FileExtension = extension;

            return await ExecuteAsync<UploadDumpCommand, Result<string>>(context, logger, req, commandArgs);
        }

    }
}
