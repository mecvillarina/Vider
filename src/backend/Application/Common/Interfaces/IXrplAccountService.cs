using Application.Common.Dtos.Response;

namespace Application.Common.Interfaces
{
    public interface IXrplAccountService
    {
        XrplAccountInfoResultDto AccountInfo(string address);
    }
}