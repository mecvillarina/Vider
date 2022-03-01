using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Common.Dtos.Response
{
    public class XrplNFTSellOfferResultDto : XrplBaseResultDto
    {
        [JsonProperty("offers")]
        public List<XrplNFTSellOfferItemDto> Offers { get; set; }

        [JsonProperty("tokenid")]
        public string Tokenid { get; set; }
    }
}
