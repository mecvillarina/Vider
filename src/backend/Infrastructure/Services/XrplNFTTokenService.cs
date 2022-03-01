using Application.Common.Dtos.Request;
using Application.Common.Dtos.Response;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class XrplNFTTokenService : XrplBaseService, IXrplNFTTokenService
    {
        public XrplNFTTokenService(IConfiguration configuration) : base(configuration)
        {
        }

        public NFTMetadata GetMetadataContent(string uri)
        {
            var client = new RestClient(uri);
            var request = new RestRequest("", Method.GET, DataFormat.Json);
            var response = client.Execute<NFTMetadata>(request);

            if (!response.IsSuccessful) return null;

            return response.Data;
        }

        public XrplAccountNFTResultDto GetAccountNFTs(string address)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new XrplApiRequestDto()
            {
                Method = "account_nfts",
                Params = new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "account", address }
                    }
                }
            };

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            var response = Execute<XrplApiResponseDto<XrplAccountNFTResultDto>>(request);
            return response.Result;
        }

        public XrplNFTSellOfferResultDto GetNftSellOffers(string tokenId)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new XrplApiRequestDto()
            {
                Method = "nft_sell_offers",
                Params = new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "tokenid", tokenId }
                    }
                }
            };

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            var response = Execute<XrplApiResponseDto<XrplNFTSellOfferResultDto>>(request);
            return response.Result;
        }

        public Result<string> Mint(string accountAddress, string accountSecret, string uriHexValue, int taxon, int flags)
        {
            var txPayload = new XrplNfTokenMintTx()
            {
                TransactionType = "NFTokenMint",
                Account = accountAddress,
                Fee = 100,
                TransferFee = 0, // 0%
                Flags = flags,
                TokenTaxon = taxon,
                Uri = uriHexValue
            };

            var signedResult = SignTx(false, accountSecret, (JObject)JToken.FromObject(txPayload));

            if (signedResult.Status == "error") return Result<string>.Fail($"Signing Payment Transaction Error: {signedResult.ErrorMessage}");

            var submitResult = SubmitTx(signedResult.TxBlob);

            if (submitResult.Status == "error") return Result<string>.Fail($"Submit Transaction Error: {submitResult.ErrorMessage}");

            do
            {
                var txResult = GetTx(submitResult.TxJson.Hash);

                if (txResult.Status == "error") return Result<string>.Fail($"Transaction Error: {txResult.ErrorMessage}");

                if (txResult.Validated)
                    break;

                Task.Delay(4000).Wait();
            } while (true);

            return Result<string>.Success(data: submitResult.TxJson.Hash);
        }

        public Result<string> CreateSellOffer(string accountAddress, string accountSecret, string tokenId, string amountInDrops, string destinationAddress)
        {
            dynamic txPayload = new ExpandoObject();
            txPayload.TransactionType = "NFTokenCreateOffer";
            txPayload.Account = accountAddress;
            txPayload.TokenID = tokenId;
            txPayload.Amount = amountInDrops;
            txPayload.Flags = 1;

            if (!string.IsNullOrEmpty(destinationAddress))
            {
                txPayload.Destination = destinationAddress;
            }

            var signedResult = SignTx(false, accountSecret, (JObject)JToken.FromObject(txPayload));

            if (signedResult.Status == "error") return Result<string>.Fail($"Signing Payment Transaction Error: {signedResult.ErrorMessage}");

            var submitResult = SubmitTx(signedResult.TxBlob);

            if (submitResult.Status == "error") return Result<string>.Fail($"Submit Transaction Error: {submitResult.ErrorMessage}");

            do
            {
                var txResult = GetTx(submitResult.TxJson.Hash);

                if (txResult.Status == "error") return Result<string>.Fail($"Transaction Error: {txResult.ErrorMessage}");

                if (txResult.Validated)
                    break;

                Task.Delay(4000).Wait();
            } while (true);

            return Result<string>.Success(data: submitResult.TxJson.Hash);
        }

        public Result<string> AcceptSellOffer(string accountAddress, string accountSecret, string tokenOfferIndex)
        {
            dynamic txPayload = new ExpandoObject();
            txPayload.TransactionType = "NFTokenAcceptOffer";
            txPayload.Account = accountAddress;
            txPayload.SellOffer = tokenOfferIndex;

            var signedResult = SignTx(false, accountSecret, (JObject)JToken.FromObject(txPayload));

            if (signedResult.Status == "error") return Result<string>.Fail($"Signing Payment Transaction Error: {signedResult.ErrorMessage}");

            var submitResult = SubmitTx(signedResult.TxBlob);

            if (submitResult.Status == "error") return Result<string>.Fail($"Submit Transaction Error: {submitResult.ErrorMessage}");

            do
            {
                var txResult = GetTx(submitResult.TxJson.Hash);

                if (txResult.Status == "error") return Result<string>.Fail($"Transaction Error: {txResult.ErrorMessage}");

                if (txResult.Validated)
                    break;

                Task.Delay(4000).Wait();
            } while (true);

            return Result<string>.Success(data: submitResult.TxJson.Hash);
        }

        public Result<string> Burn(string accountAddress, string accountSecret, string tokenId)
        {
            dynamic txPayload = new ExpandoObject();
            txPayload.TransactionType = "NFTokenBurn";
            txPayload.Account = accountAddress;
            txPayload.TokenID = tokenId;

            var signedResult = SignTx(false, accountSecret, (JObject)JToken.FromObject(txPayload));

            if (signedResult.Status == "error") return Result<string>.Fail($"Signing Payment Transaction Error: {signedResult.ErrorMessage}");

            var submitResult = SubmitTx(signedResult.TxBlob);

            if (submitResult.Status == "error") return Result<string>.Fail($"Submit Transaction Error: {submitResult.ErrorMessage}");

            do
            {
                var txResult = GetTx(submitResult.TxJson.Hash);

                if (txResult.Status == "error") return Result<string>.Fail($"Transaction Error: {txResult.ErrorMessage}");

                if (txResult.Validated)
                    break;

                Task.Delay(4000).Wait();
            } while (true);

            return Result<string>.Success(data: submitResult.TxJson.Hash);
        }


        public Result<string> CancelOffer(string accountAddress, string accountSecret, List<string> tokenIds)
        {
            dynamic txPayload = new ExpandoObject();
            txPayload.TransactionType = "NFTokenCancelOffer";
            txPayload.Account = accountAddress;
            txPayload.TokenOffers = tokenIds.ToArray();

            var signedResult = SignTx(false, accountSecret, (JObject)JToken.FromObject(txPayload));

            if (signedResult.Status == "error") return Result<string>.Fail($"Signing Payment Transaction Error: {signedResult.ErrorMessage}");

            var submitResult = SubmitTx(signedResult.TxBlob);

            if (submitResult.Status == "error") return Result<string>.Fail($"Submit Transaction Error: {submitResult.ErrorMessage}");

            do
            {
                var txResult = GetTx(submitResult.TxJson.Hash);

                if (txResult.Status == "error") return Result<string>.Fail($"Transaction Error: {txResult.ErrorMessage}");

                if (txResult.Validated)
                    break;

                Task.Delay(4000).Wait();
            } while (true);

            return Result<string>.Success(data: submitResult.TxJson.Hash);
        }
    }
}
