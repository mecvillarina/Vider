using Application.Common.Mappings;
using Application.Common.Models;
using Domain.Entities;

namespace Application.CreatorPortal.NFTs.Dtos
{
    public class NFTClaimDto : IMapFrom<NFTClaim>
    {
        public int Id { get; set; }
        public string TokenId { get; set; }
        public NFTMetadata Metadata { get; set; }
        public string SenderMessage { get; set; }
        public string SenderUsername { get; set; }
    }
}
