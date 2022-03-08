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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface INFTWebService : IWebService
    {
        Task<IResult<string>> GetTxAsync(string txHash, string accessToken);
        Task<IResult<List<NFTItemDto>>> GetCreatorNFTsAsync(int creatorId, string accessToken);
        Task<IResult<List<NFTClaimDto>>> GetNFTClaimsAsync(string accessToken);
        Task<IResult<List<NFTSellOfferItemDto>>> GetNFTSellOffersAsync(string query, string accessToken);
        Task<IResult> ClaimNFTAsync(ClaimNFTCommand request, string accessToken);
        Task<IResult> BurnNFTAsync(BurnNFTCommand request, string accessToken);
        Task<IResult> GiftNFTAsync(GiftNFTCommand request, string accessToken);
        Task<IResult> MintNFTAsync(MintNFTCommand request, string accessToken);
        Task<IResult> SellNFTAsync(SellNFTCommand request, string accessToken);
        Task<IResult> CancelSellNFTAsync(CancelSellNFTCommand request, string accessToken);
        Task<IResult> BuyNFTAsync(BuyNFTCommand request, string accessToken);
    }
}