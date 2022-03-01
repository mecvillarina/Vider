using Application.Common.Dtos.Request;
using Application.Common.Dtos.Response;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class XrplAccountService : XrplBaseService, IXrplAccountService
    {
        public XrplAccountService(IConfiguration configuration) : base(configuration)
        {
        }

        public XrplAccountInfoResultDto AccountInfo(string address)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new XrplApiRequestDto()
            {
                Method = "account_info",
                Params = new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "account", address },
                        { "strict", true },
                        { "ledger_index", "current"},
                        { "queue", true},
                    }
                }
            };

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            var response = Execute<XrplApiResponseDto<XrplAccountInfoResultDto>>(request);
            return response.Result;
        }
    }
}
