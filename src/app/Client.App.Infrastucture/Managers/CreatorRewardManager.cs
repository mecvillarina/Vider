using Application.Common.Models;
using Application.CreatorPortal.CreatorRewards.Commands.Delete;
using Application.CreatorPortal.CreatorRewards.Commands.Upload;
using Application.CreatorPortal.CreatorRewards.Queries.GetRewards;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CreatorRewardManager : ManagerBase, ICreatorRewardManager
    {
        private readonly ICreatorRewardWebService _creatorRewardWebService;

        public CreatorRewardManager(IManagerToolkit managerToolkit, ICreatorRewardWebService creatorRewardWebService)
            : base(managerToolkit)
        {
            _creatorRewardWebService = creatorRewardWebService;
        }

        public async Task<IResult<List<GetRewardItemDto>>> GetRewardsAsync(int creatorId = 0)
        {
            await PrepareForWebserviceCall();
            return await _creatorRewardWebService.GetRewardsAsync(creatorId, AccessToken);
        }

        public async Task<IResult> UploadRewardAsync(UploadRewardCommand request, Stream fileStream, string filename)
        {
            await PrepareForWebserviceCall();
            return await _creatorRewardWebService.UploadRewardAsync(request, fileStream, filename, AccessToken);
        }

        public async Task<IResult> DeleteRewardAsync(int rewardId)
        {
            await PrepareForWebserviceCall();
            return await _creatorRewardWebService.DeleteRewardAsync(new DeleteRewardCommand() { RewardId = rewardId }, AccessToken);
        }
    }
}