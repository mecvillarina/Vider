using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplAccountInfoResultDto : XrplBaseResultDto
    {
        [JsonProperty("account_data")]
        public XrplAccountInfoDataItemDto AccountData { get; set; }

        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public long LedgerIndex { get; set; }
    }
}
