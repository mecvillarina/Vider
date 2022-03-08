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
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class NFTManager : ManagerBase, INFTManager
    {
        private readonly INFTWebService _nftWebService;
        public NFTManager(IManagerToolkit managerToolkit, INFTWebService creatorNftWebService) : base(managerToolkit)
        {
            _nftWebService = creatorNftWebService;
        }

        public async Task<IResult<string>> GetTxAsync(string txHash)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.GetTxAsync(txHash, AccessToken);
        }

        public async Task<IResult<List<NFTItemDto>>> GetCreatorNFTsAsync(int creatorId = 0)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.GetCreatorNFTsAsync(creatorId, AccessToken);
        }

        public async Task<IResult<List<NFTClaimDto>>> GetNFTClaimsAsync()
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.GetNFTClaimsAsync(AccessToken);
        }

        public async Task<IResult<List<NFTSellOfferItemDto>>> GetNFTSellOffersAsync(string query = null)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.GetNFTSellOffersAsync(query, AccessToken);
        }

        public async Task<IResult> ClaimNFTAsync(ClaimNFTCommand request)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.ClaimNFTAsync(request, AccessToken);
        }

        public async Task<IResult> BurnNFTAsync(BurnNFTCommand request)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.BurnNFTAsync(request, AccessToken);
        }

        public async Task<IResult> GiftNFTAsync(GiftNFTCommand request)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.GiftNFTAsync(request, AccessToken);
        }

        public async Task<IResult> MintNFTAsync(MintNFTCommand request)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.MintNFTAsync(request, AccessToken);
        }

        public async Task<IResult> SellNFTAsync(SellNFTCommand request)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.SellNFTAsync(request, AccessToken);
        }

        public async Task<IResult> CancelSellNFTAsync(CancelSellNFTCommand request)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.CancelSellNFTAsync(request, AccessToken);
        }

        public async Task<IResult> BuyNFTAsync(BuyNFTCommand request)
        {
            await PrepareForWebserviceCall();
            return await _nftWebService.BuyNFTAsync(request, AccessToken);
        }
    }
}
