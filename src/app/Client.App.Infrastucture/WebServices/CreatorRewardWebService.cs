using Application.Common.Models;
using Application.CreatorPortal.CreatorRewards.Commands.Delete;
using Application.CreatorPortal.CreatorRewards.Commands.Upload;
using Application.CreatorPortal.CreatorRewards.Queries.GetRewards;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CreatorRewardWebService : WebServiceBase, ICreatorRewardWebService
    {
        public CreatorRewardWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<GetRewardItemDto>>> GetRewardsAsync(int creatorId, string accessToken) => GetAsync<List<GetRewardItemDto>>(string.Format(CreatorRewardEndpoints.GetRewards, creatorId), accessToken);
        public Task<IResult> UploadRewardAsync(UploadRewardCommand request, Stream fileStream, string filename, string accessToken) => PostFileAsync(CreatorRewardEndpoints.UploadReward, request, fileStream, filename, accessToken);
        public Task<IResult> DeleteRewardAsync(DeleteRewardCommand request, string accessToken) => PostAsync(CreatorRewardEndpoints.DeleteReward, request, accessToken);
    }
}
