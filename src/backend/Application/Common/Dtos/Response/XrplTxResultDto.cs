using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplTxResultDto : XrplBaseResultDto
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }

        [JsonProperty("ledger_index")]
        public int LedgerIndex { get; set; }

        [JsonProperty("meta")]
        public dynamic Meta { get; set; }
    }
}
