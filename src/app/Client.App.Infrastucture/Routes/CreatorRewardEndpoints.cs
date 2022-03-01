using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Routes
{
    public static class CreatorRewardEndpoints
    {
        public const string GetRewards = "api/creatorportal/creator/rewards?creatorId={0}";
        public const string UploadReward = "api/creatorportal/creator/reward";
        public const string DeleteReward = "api/creatorportal/creator/reward/delete";
    }
}
