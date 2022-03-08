using Application.Common.Dtos.Response;
using Application.Common.Models;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IXrplNFTTokenService
    {
        XrplTxResultDto GetTx(string hash);
        NFTMetadata GetMetadataContent(string uri);
        XrplAccountNFTResultDto GetAccountNFTs(string address);
        XrplNFTSellOfferResultDto GetNftSellOffers(string tokenId);
        Result<string> Mint(string accountAddress, string accountSecret, string uriHexValue, int taxon, int flags);
        Result<string> CreateSellOffer(string accountAddress, string accountSecret, string tokenId, string amountInDrops, string destinationAddress);
        Result<string> AcceptSellOffer(string accountAddress, string accountSecret, string tokenOfferIndex);
        Result<string> Burn(string accountAddress, string accountSecret, string tokenId);
        Result<string> CancelOffer(string accountAddress, string accountSecret, List<string> tokenIds);
    }
}