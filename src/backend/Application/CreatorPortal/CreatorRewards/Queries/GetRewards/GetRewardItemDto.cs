using Application.Common.Mappings;
using Domain.Entities;

namespace Application.CreatorPortal.CreatorRewards.Queries.GetRewards
{
    public class GetRewardItemDto : IMapFrom<CreatorReward>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlLink { get; set; }
        public string Message { get; set; }
        public int Taxon { get; set; }
    }
}
