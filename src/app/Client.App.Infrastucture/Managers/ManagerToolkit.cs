using Application.Common.Models;
using Application.CreatorPortal.Account.Queries.GetWallet;
using Application.CreatorPortal.Account.Queries.MyProfile;
using Blazored.LocalStorage;
using Client.Infrastructure.Constants;
using Client.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class ManagerToolkit : IManagerToolkit
    {
        private readonly ILocalStorageService _localStorage;

        public ManagerToolkit(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveAuthTokenHandler(AuthTokenHandler authTokenHandler) => await _localStorage.SetItemAsync(StorageConstants.Local.AuthTokenHandler, authTokenHandler);

        public async Task ClearAuthTokenHandler()
        {
            await _localStorage.RemoveItemAsync(StorageConstants.Local.Wallet);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.Profile);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthTokenHandler);
        }
        public async Task<string> GetAuthToken()
        {
            var tokenHandler = await _localStorage.GetItemAsync<AuthTokenHandler>(StorageConstants.Local.AuthTokenHandler);

            if (tokenHandler == null || !tokenHandler.IsValid())
            {
                await ClearAuthTokenHandler();
                throw new SessionExpiredException();
            }

            return tokenHandler.Token;
        }

        public async Task SaveProfile(MyProfileResponse data) => await _localStorage.SetItemAsync(StorageConstants.Local.Profile, data);
        public async Task<MyProfileResponse> GetProfile()
        {
            var data = await _localStorage.GetItemAsync<MyProfileResponse>(StorageConstants.Local.Profile);
            return data;
        }

        public async Task SaveWallet(GetWalletResponse data) => await _localStorage.SetItemAsync(StorageConstants.Local.Wallet, data);
        public async Task<GetWalletResponse> GetWallet()
        {
            var data = await _localStorage.GetItemAsync<GetWalletResponse>(StorageConstants.Local.Wallet);
            return data;
        }
    }
}
