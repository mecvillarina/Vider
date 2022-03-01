using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Common.Dtos.Request
{
    public class XrplApiRequestDto
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public List<Dictionary<string,object>> Params { get; set; }
    }
}
