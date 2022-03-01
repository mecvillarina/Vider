using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.Response
{
    public class XrplAccountNFTResultDto : XrplBaseResultDto
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("account_nfts")]
        public List<XrplAccountNFTItemDto> AccountNfts { get; set; }

        [JsonProperty("ledger_current_index")]
        public long LedgerCurrentIndex { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
}
