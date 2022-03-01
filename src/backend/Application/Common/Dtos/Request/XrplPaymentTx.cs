using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.Request
{
    public class XrplPaymentTx
    {
        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("Amount")]
        public string Amount { get; set; }

        [JsonProperty("Destination")]
        public string Destination { get; set; }
    }
}
