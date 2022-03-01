using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.Response
{
    public class XrplSubmitResultDto : XrplBaseResultDto
    {
        [JsonProperty("tx_blob")]
        public string TxBlob { get; set; }

        [JsonProperty("tx_json")]
        public XrplSubmitTxResultDto TxJson { get; set; }
    }
}
