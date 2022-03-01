using Application.Common.Dtos.Request;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class XrplPaymentService : XrplBaseService, IXrplPaymentService
    {
        public XrplPaymentService(IConfiguration configuration) : base(configuration)
        {
        }

        public Result<string> Pay(string accountAddress, string accountSecret, string amountInDrops, string destinationAddress)
        {
            var paymentTx = new XrplPaymentTx()
            {
                TransactionType = "Payment",
                Account = accountAddress,
                Amount = amountInDrops,
                Destination = destinationAddress
            };

            var signedResult = SignTx(false, accountSecret, (JObject)JToken.FromObject(paymentTx));

            if (signedResult.Status == "error") return Result<string>.Fail($"Signing Payment Transaction Error: {signedResult.ErrorMessage}");

            var submitResult = SubmitTx(signedResult.TxBlob);

            if (submitResult.Status == "error") return Result<string>.Fail($"Submit Transaction Error: {submitResult.ErrorMessage}");

            do
            {
                var txResult = GetTx(submitResult.TxJson.Hash);

                if (txResult.Status == "error") return Result<string>.Fail($"Transaction Error: {txResult.ErrorMessage}");

                if (txResult.Validated)
                    break;

                Task.Delay(4000).Wait();
            } while (true);

            return Result<string>.Success(data: submitResult.TxJson.Hash);
        }
    }
}
