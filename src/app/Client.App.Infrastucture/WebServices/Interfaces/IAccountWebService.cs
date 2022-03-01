using Application.Common.Models;
using Application.CreatorPortal.Account.Commands.Login;
using Application.CreatorPortal.Account.Commands.PopulateWallet;
using Application.CreatorPortal.Account.Commands.Register;
using Application.CreatorPortal.Account.Commands.UploadProfilePicture;
using Application.CreatorPortal.Account.Queries.GetWallet;
using Application.CreatorPortal.Account.Queries.MyProfile;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IAccountWebService : IWebService
    {
        Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request);
        Task<IResult> RegisterAsync(RegisterCommand request);
        Task<IResult<MyProfileResponse>> GetProfileAsync(string accessToken);
        Task<IResult> UploadPhotoAsync(UploadProfilePictureCommand request, Stream fileStream, string filename, string accessToken);
        Task<IResult<GetWalletResponse>> GetWalletAsync(string accessToken);
        Task<IResult> PopulateWalletAsync(PopulateWalletCommand request, string accessToken);

    }
}