using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using Application.Queues.Commands.MintNftSubscribeReward;
using MediatR;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using WebJob.Base;

namespace WebJob.Queues
{
    public class GeneralQueues : FunctionBase
    {
        public GeneralQueues(IMediator mediator, ICallContext context) : base(mediator, context)
        {
        }

        [FunctionName("GeneralQueues_MintNFTSubscribeReward")]
        public async Task InviteApplicantSubscriber([QueueTrigger(QueueNames.MintNFTSubscribeReward)] MintNFTSubscribeRewardQueueMessage message, ExecutionContext context)
        {
            MintNFTSubscribeRewardCommand commandArg = new() { Message = message };
            await ExecuteAsync<MintNFTSubscribeRewardCommand, IResult>(context, commandArg);
        }

    }
}
