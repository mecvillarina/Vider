using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplSubmitTxResultDto
    {
        [JsonProperty("TxnSignature")]
        public string TxnSignature { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
