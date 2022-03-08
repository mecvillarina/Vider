using Application.Common.Models;
using Application.CreatorPortal.Account.Commands.Login;
using Application.CreatorPortal.Account.Commands.PopulateWallet;
using Application.CreatorPortal.Account.Commands.Register;
using Application.CreatorPortal.Account.Commands.UploadProfilePicture;
using Application.CreatorPortal.Account.Queries.GetWallet;
using Application.CreatorPortal.Account.Queries.MyProfile;
using Application.CreatorPortal.Activities.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class AccountWebService : WebServiceBase, IAccountWebService
    {
        public AccountWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request) => PostAsync<LoginCommand, LoginCommandResponse>(AccountEndpoints.Login, request);
        public Task<IResult> RegisterAsync(RegisterCommand request) => PostAsync(AccountEndpoints.Register, request);
        public Task<IResult<MyProfileResponse>> GetProfileAsync(string accessToken) => GetAsync<MyProfileResponse>(AccountEndpoints.Profile, accessToken);
        public Task<IResult> UploadPhotoAsync(UploadProfilePictureCommand request, Stream fileStream, string filename, string accessToken) => PostFileAsync(AccountEndpoints.UploadPhoto, request, fileStream, filename, accessToken);
        public Task<IResult<GetWalletResponse>> GetWalletAsync(string accessToken) => GetAsync<GetWalletResponse>(AccountEndpoints.Wallet, accessToken);
        public Task<IResult> PopulateWalletAsync(PopulateWalletCommand request, string accessToken) => PostAsync(AccountEndpoints.PopulateWallet, request, accessToken);
        public Task<IResult<List<ActivityLogDto>>> GetRecentActivitiesAsync(string accessToken) => GetAsync<List<ActivityLogDto>>(AccountEndpoints.GetRecentActivities, accessToken);

    }
}
