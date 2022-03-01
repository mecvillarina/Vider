using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplAccountNFTItemDto
    {
        [JsonProperty("Flags")]
        public int Flags { get; set; }

        [JsonProperty("Issuer")]
        public string Issuer { get; set; }

        [JsonProperty("TokenID")]
        public string TokenId { get; set; }

        [JsonProperty("TokenTaxon")]
        public int TokenTaxon { get; set; }

        [JsonProperty("URI")]
        public string Uri { get; set; }

        [JsonProperty("nft_serial")]
        public int NftSerial { get; set; }
    }
}
