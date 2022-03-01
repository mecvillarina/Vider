using Application.Common.Dtos.Response;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Faucet.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.CreatorPortal
{
    public class FaucetController : HttpFunctionBase
    {
        public FaucetController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("CreatorPortal_Faucet_GenerateAccount")]
        public async Task<IActionResult> GenerateAccount([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/faucet/account")] GenerateAccountCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<GenerateAccountCommand, Result<FaucetResponseDto>>(context, logger, req, commandArg);
        }
    }
}
