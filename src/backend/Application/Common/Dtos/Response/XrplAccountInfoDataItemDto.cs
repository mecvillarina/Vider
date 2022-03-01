using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplAccountInfoDataItemDto
    {
        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("Balance")]
        public string Balance { get; set; }

        [JsonProperty("Flags")]
        public long Flags { get; set; }

        [JsonProperty("LedgerEntryType")]
        public string LedgerEntryType { get; set; }

        [JsonProperty("OwnerCount")]
        public long OwnerCount { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTxnId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public long PreviousTxnLgrSeq { get; set; }

        [JsonProperty("Sequence")]
        public long Sequence { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }
    }
}
