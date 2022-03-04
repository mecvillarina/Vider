using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Creators.Commands.WalletChecker;
using Application.CreatorPortal.NFTs.Commands.NFTSellOffersCheck;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebJob.Base;

namespace WebJob.Timers
{
    public class GeneralTimers : FunctionBase
    {
        public GeneralTimers(IMediator mediator, ICallContext context) : base(mediator, context)
        {
        }

        [FunctionName("GeneralTimers_NFTSellOffersCheck")]
        public async Task NFTSellOffersCheck([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            NFTSellOffersCheckCommand commandArg = new();
            await ExecuteAsync<NFTSellOffersCheckCommand, IResult>(context, commandArg);
        }


        [FunctionName("GeneralTimers_WalletChecker")]
        public async Task WalletChecker([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            WalletCheckerCommand commandArg = new();
            await ExecuteAsync<WalletCheckerCommand, IResult>(context, commandArg);
        }
    }
}
