using Application.Common.Dtos.Response;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.NFTs.Commands.BurnNFT;
using Application.CreatorPortal.NFTs.Commands.BuyNFT;
using Application.CreatorPortal.NFTs.Commands.CancelSellNFT;
using Application.CreatorPortal.NFTs.Commands.ClaimNFT;
using Application.CreatorPortal.NFTs.Commands.GiftNFT;
using Application.CreatorPortal.NFTs.Commands.MintNFT;
using Application.CreatorPortal.NFTs.Commands.SellNFT;
using Application.CreatorPortal.NFTs.Dtos;
using Application.CreatorPortal.NFTs.Queries.GetCreatorNFTs;
using Application.CreatorPortal.NFTs.Queries.GetNFTClaims;
using Application.CreatorPortal.NFTs.Queries.GetNFTSellOffers;
using Application.CreatorPortal.NFTs.Queries.GetTx;
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
    public class NFTController : HttpFunctionBase
    {
        public NFTController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("CreatorPortal_NFT_TX")]
        public async Task<IActionResult> GetTx([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/nfts/tx")] GetTxQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetTxQuery, Result<string>>(context, logger, req, queryArgs);
        }

        [FunctionName("CreatorPortal_NFT_GetCreatorNFTs")]
        public async Task<IActionResult> GetCreatorNFTs([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/nfts/creatorNFTs")] GetCreatorNFTsQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetCreatorNFTsQuery, Result<List<NFTItemDto>>>(context, logger, req, queryArgs);
        }

        [FunctionName("CreatorPortal_NFT_GetNFTClaims")]
        public async Task<IActionResult> GetNFTClaims([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/nfts/claims")] GetNFTClaimsQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetNFTClaimsQuery, Result<List<NFTClaimDto>>>(context, logger, req, queryArgs);
        }

        [FunctionName("CreatorPortal_NFT_GetNFTSellOffers")]
        public async Task<IActionResult> GetNFTSellOffers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "creatorportal/nfts/sellOffers")] GetNFTSellOffersQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetNFTSellOffersQuery, Result<List<NFTSellOfferItemDto>>>(context, logger, req, queryArgs);
        }

        [FunctionName("CreatorPortal_NFT_Claim")]
        public async Task<IActionResult> ClaimNFT([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/nfts/claim")] ClaimNFTCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<ClaimNFTCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_NFT_Burn")]
        public async Task<IActionResult> BurnNFT([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/nfts/burn")] BurnNFTCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<BurnNFTCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_NFT_Gift")]
        public async Task<IActionResult> GiftNFT([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/nfts/gift")] GiftNFTCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GiftNFTCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_NFT_Mint")]
        public async Task<IActionResult> MintNFT([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/nfts/mint")] MintNFTCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<MintNFTCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_NFT_Sell")]
        public async Task<IActionResult> SellNFT([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/nfts/sell")] SellNFTCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<SellNFTCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_NFT_CancelSell")]
        public async Task<IActionResult> CancelSellNFT([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/nfts/cancelsell")] CancelSellNFTCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<CancelSellNFTCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("CreatorPortal_NFT_Buy")]
        public async Task<IActionResult> BuyNFT([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "creatorportal/nfts/buy")] BuyNFTCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<BuyNFTCommand, IResult>(context, logger, req, commandArgs);
        }
    }
}
