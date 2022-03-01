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

namespace Client.App.Infrastructure.Managers
{
    public interface INFTManager : IManager
    {
        Task<IResult<List<NFTItemDto>>> GetCreatorNFTsAsync(int creatorId = 0);
        Task<IResult<List<NFTClaimDto>>> GetNFTClaimsAsync();
        Task<IResult<List<NFTSellOfferItemDto>>> GetNFTSellOffersAsync(string query = null);
        Task<IResult> ClaimNFTAsync(ClaimNFTCommand request);
        Task<IResult> BurnNFTAsync(BurnNFTCommand request);
        Task<IResult> GiftNFTAsync(GiftNFTCommand request);
        Task<IResult> MintNFTAsync(MintNFTCommand request);
        Task<IResult> SellNFTAsync(SellNFTCommand request);
        Task<IResult> CancelSellNFTAsync(CancelSellNFTCommand request);
        Task<IResult> BuyNFTAsync(BuyNFTCommand request);
    }
}