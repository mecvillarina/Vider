using Application.Common.Models;
using Application.CreatorPortal.Account.Commands.Login;
using Application.CreatorPortal.Account.Commands.Register;
using Application.CreatorPortal.Account.Queries.GetWallet;
using Application.CreatorPortal.Account.Queries.MyProfile;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IAccountManager : IManager
    {
        Task<ClaimsPrincipal> CurrentUser();
        Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request);
        Task<IResult> RegisterAsync(RegisterCommand request);
        Task<IResult<MyProfileResponse>> GetProfileAsync();
        Task<MyProfileResponse> FetchProfileAsync();
        Task<IResult<GetWalletResponse>> GetWalletAsync();
        Task<GetWalletResponse> FetchWalletAsync();
        Task<IResult> PopulateWalletAsync();
        Task<IResult> UploadPhotoAsync(Stream fileStream, string filename);
        Task<IResult> LogoutAsync();
    }
}