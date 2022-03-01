using Application.Common.Constants;
using Application.Common.Dtos.Request;
using Application.Common.Dtos.Response;
using Application.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class XrplBaseService
    {
        private readonly IRestClient _client;
        public XrplBaseService(IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>(SettingKeys.XPRLEndpoint);
            _client = new RestClient(baseUrl);
        }

        public void Execute(RestRequest request)
        {
            var response = _client.Execute(request);
            ProcessResponse(response);
        }

        public T Execute<T>(RestRequest request)
        {
            var response = _client.Execute<T>(request);
            ProcessResponse(response);
            return response.Data;
        }

        private void ProcessResponse(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                throw new XRPLServerErrorException("There's a problem on XRPL JSON-RPC Service. Please try again.");
            }
        }

        public XrplSignResultDto SignTx(bool offline, string secret, JObject txJson)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new XrplApiRequestDto()
            {
                Method = "sign",
                Params = new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "sign", offline },
                        { "secret", secret },
                        { "fee_mult_max", 1000},
                        { "tx_json", txJson},
                    }
                }
            };

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            var response = Execute<XrplApiResponseDto<XrplSignResultDto>>(request);
            return response.Result;
        }

        public XrplSubmitResultDto SubmitTx(string txBlob)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new XrplApiRequestDto()
            {
                Method = "submit",
                Params = new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "tx_blob", txBlob }
                    }
                }
            };

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            var response = Execute<XrplApiResponseDto<XrplSubmitResultDto>>(request);
            return response.Result;
        }

        public XrplTxResultDto GetTx(string hash)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new XrplApiRequestDto()
            {
                Method = "tx",
                Params = new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "transaction", hash },
                        { "binary", false }
                    }
                }
            };

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            var response = Execute<XrplApiResponseDto<XrplTxResultDto>>(request);
            return response.Result;
        }
    }
}
