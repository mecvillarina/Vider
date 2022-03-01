using Newtonsoft.Json;

namespace Application.Common.Dtos.Request
{
    public class XrplNfTokenMintTx
    {
        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("TransferFee")]
        public int TransferFee { get; set; }

        [JsonProperty("TokenTaxon")]
        public int TokenTaxon { get; set; }

        [JsonProperty("Flags")]
        public int Flags { get; set; }

        [JsonProperty("Fee")]
        public long Fee { get; set; }

        [JsonProperty("URI")]
        public string Uri { get; set; }
    }
}
