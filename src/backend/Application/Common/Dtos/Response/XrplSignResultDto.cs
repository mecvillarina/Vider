using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Common.Dtos.Response
{
    public class XrplSignResultDto : XrplBaseResultDto
    {
        [JsonProperty("tx_blob")]
        public string TxBlob { get; set; }

        [JsonProperty("tx_json")]
        public dynamic Tx { get; set; }
    }
}
