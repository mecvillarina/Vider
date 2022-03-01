using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class FaucetAccountDto
    {
        [JsonProperty("xAddress")]
        public string XAddress { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("classicAddress")]
        public string ClassicAddress { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
