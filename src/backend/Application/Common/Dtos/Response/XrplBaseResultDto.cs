using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplBaseResultDto
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_code")]
        public long ErrorCode { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
