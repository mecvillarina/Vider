using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class FaucetResponseDto
    {
        [JsonProperty("account")]
        public FaucetAccountDto Account { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("balance")]
        public long Balance { get; set; }
    }
}
