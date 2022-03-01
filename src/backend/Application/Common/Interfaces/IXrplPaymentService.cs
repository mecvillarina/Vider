using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IXrplPaymentService
    {
        Result<string> Pay(string accountAddress, string accountSecret, string amountInDrops, string destinationAddress);
    }
}