using Application.Common.Dtos.Response;
using Application.Common.Models;
using Application.CreatorPortal.NFTs.Commands.BurnNFT;
using Application.CreatorPortal.NFTs.Commands.BuyNFT;
using Application.CreatorPortal.NFTs.Commands.CancelSellNFT;
using Application.CreatorPortal.NFTs.Commands.ClaimNFT;
using Application.CreatorPortal.NFTs.Commands.GiftNFT;
using Application.CreatorPortal.NFTs.Commands.MintNFT;
using Application.CreatorPortal.NFTs.Commands.SellNFT;
using Application.CreatorPortal.NFTs.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class NFTWebService : WebServiceBase, INFTWebService
    {
        public NFTWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<string>> GetTxAsync(string txHash, string accessToken) => GetAsync<string>(string.Format(NFTEndpoints.GetTx, txHash), accessToken);
        public Task<IResult<List<NFTItemDto>>> GetCreatorNFTsAsync(int creatorId, string accessToken) => GetAsync<List<NFTItemDto>>(string.Format(NFTEndpoints.GetCreatorNFTs, creatorId), accessToken);
        public Task<IResult<List<NFTClaimDto>>> GetNFTClaimsAsync(string accessToken) => GetAsync<List<NFTClaimDto>>(NFTEndpoints.GetNFTClaims, accessToken);
        public Task<IResult<List<NFTSellOfferItemDto>>> GetNFTSellOffersAsync(string query, string accessToken) => GetAsync<List<NFTSellOfferItemDto>>(string.Format(NFTEndpoints.GetNFTSellOffers, query), accessToken);
        public Task<IResult> ClaimNFTAsync(ClaimNFTCommand request, string accessToken) => PostAsync(NFTEndpoints.ClaimNFT, request, accessToken);
        public Task<IResult> BurnNFTAsync(BurnNFTCommand request, string accessToken) => PostAsync(NFTEndpoints.BurnNFT, request, accessToken);
        public Task<IResult> GiftNFTAsync(GiftNFTCommand request, string accessToken) => PostAsync(NFTEndpoints.GiftNFT, request, accessToken);
        public Task<IResult> MintNFTAsync(MintNFTCommand request, string accessToken) => PostAsync(NFTEndpoints.MintNFT, request, accessToken);
        public Task<IResult> SellNFTAsync(SellNFTCommand request, string accessToken) => PostAsync(NFTEndpoints.SellNFT, request, accessToken);
        public Task<IResult> CancelSellNFTAsync(CancelSellNFTCommand request, string accessToken) => PostAsync(NFTEndpoints.CancelSellNFT, request, accessToken);
        public Task<IResult> BuyNFTAsync(BuyNFTCommand request, string accessToken) => PostAsync(NFTEndpoints.BuyNFT, request, accessToken);

    }
}
