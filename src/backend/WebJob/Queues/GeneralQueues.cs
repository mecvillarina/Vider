using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using Application.CreatorPortal.NFTs.Commands.NFTSellOfferCleanup;
using Application.Queues.Commands.MintNftSubscribeReward;
using Application.Queues.Commands.WalletChecker;
using Domain.Entities;
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

        [FunctionName("GeneralQueues_NFTSellOfferCleanup")]
        public async Task NFTSellOfferCleanup([QueueTrigger(QueueNames.CheckNFTSellerOffers)] NFTSellOfferItem message, ExecutionContext context)
        {
            NFTSellOfferCleanupCommand commandArg = new() { Message = message };
            await ExecuteAsync<NFTSellOfferCleanupCommand, IResult>(context, commandArg);
        }

        [FunctionName("GeneralQueues_WalletChecker")]
        public async Task WalletChecker([QueueTrigger(QueueNames.WalletChecker)] WalletCheckerQueueMessage message, ExecutionContext context)
        {
            WalletCheckerQueueCommand commandArg = new() { Message = message };
            await ExecuteAsync<WalletCheckerQueueCommand, IResult>(context, commandArg);
        }
    }
}
