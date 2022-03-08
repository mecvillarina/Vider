using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Account.Commands.Login;
using Application.CreatorPortal.Account.Commands.PopulateWallet;
using Application.CreatorPortal.Account.Commands.Register;
using Application.CreatorPortal.Account.Commands.UploadProfilePicture;
using Application.CreatorPortal.Account.Queries.GetWallet;
using Application.CreatorPortal.Account.Queries.MyProfile;
using Application.CreatorPortal.Activities.Dtos;
using Application.CreatorPortal.Activities.Queries.GetRecent;
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
    public class AccountController : HttpFunctionBase
    {
        public AccountController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("CreatorPortal_Account_Login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/account/login")] LoginCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<LoginCommand, Result<LoginCommandResponse>>(context, logger, req, commandArg);
        }

        [FunctionName("CreatorPortal_Account_Register")]
        public async Task<IActionResult> Register([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/account/register")] RegisterCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<RegisterCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("CreatorPortal_Account_Profile_Get")]
        public async Task<IActionResult> GetProfile([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/account/profile")] MyProfileQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<MyProfileQuery, Result<MyProfileResponse>>(context, logger, req, queryArgs);
        }

        [FunctionName("CreatorPortal_Account_Profile_UploadPhoto")]
        public async Task<IActionResult> UploadPhoto([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/account/profile/photo")] UploadProfilePictureCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);

            commandArgs ??= new UploadProfilePictureCommand();

            string name = $"AttachedFile";
            var file = req.Form.Files.GetFile(name);

            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();
            string extension = Path.GetExtension(filename);

            commandArgs.FileStream = stream;
            commandArgs.FileExtension = extension;

            return await ExecuteAsync<UploadProfilePictureCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Account_Wallet")]
        public async Task<IActionResult> GetWallet([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/account/wallet")] GetWalletQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetWalletQuery, Result<GetWalletResponse>>(context, logger, req, queryArgs);
        }

        [FunctionName("CreatorPortal_Account_Wallet_Populate")]
        public async Task<IActionResult> PopulateWallet([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/account/wallet/populate")] PopulateWalletCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<PopulateWalletCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_Account_RecentActivities")]
        public async Task<IActionResult> GetRecentActivities([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/account/activity")] GetRecentActivityLogsQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetRecentActivityLogsQuery, Result<List<ActivityLogDto>>>(context, logger, req, queryArgs);
        }


    }
}
