using Application.Common.Models;
using Application.CreatorPortal.CreatorRewards.Commands.Delete;
using Application.CreatorPortal.CreatorRewards.Commands.Upload;
using Application.CreatorPortal.CreatorRewards.Queries.GetRewards;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICreatorRewardWebService : IWebService
    {
        Task<IResult<List<GetRewardItemDto>>> GetRewardsAsync(int creatorId, string accessToken);
        Task<IResult> UploadRewardAsync(UploadRewardCommand request, Stream fileStream, string filename, string accessToken);
        Task<IResult> DeleteRewardAsync(DeleteRewardCommand request, string accessToken);
    }
}