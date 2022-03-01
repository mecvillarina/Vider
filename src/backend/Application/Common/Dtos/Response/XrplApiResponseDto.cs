using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class XrplApiResponseDto<T> where T : XrplBaseResultDto
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
