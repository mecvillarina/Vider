using Application.Common.Models;
using Application.CreatorPortal.Account.Queries.GetWallet;
using Application.CreatorPortal.Account.Queries.MyProfile;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IManagerToolkit : IManager
    {
        Task ClearAuthTokenHandler();
        Task SaveAuthTokenHandler(AuthTokenHandler authTokenHandler);
        Task<string> GetAuthToken();

        Task SaveProfile(MyProfileResponse data);
        Task<MyProfileResponse> GetProfile();

        Task SaveWallet(GetWalletResponse data);
        Task<GetWalletResponse> GetWallet();
    }
}