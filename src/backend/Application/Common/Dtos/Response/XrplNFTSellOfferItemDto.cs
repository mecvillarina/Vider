using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplNFTSellOfferItemDto
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("flags")]
        public long Flags { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }
    }
}
