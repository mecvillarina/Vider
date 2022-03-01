using Application.Common.Models;
using Application.CreatorPortal.CreatorRewards.Commands.Upload;
using Application.CreatorPortal.CreatorRewards.Queries.GetRewards;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICreatorRewardManager : IManager
    {
        Task<IResult<List<GetRewardItemDto>>> GetRewardsAsync(int creatorId = 0);
        Task<IResult> UploadRewardAsync(UploadRewardCommand request, Stream fileStream, string filename);
        Task<IResult> DeleteRewardAsync(int rewardId);
    }
}