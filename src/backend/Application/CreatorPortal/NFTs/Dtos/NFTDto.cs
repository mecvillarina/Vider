using Application.Common.Models;

namespace Application.CreatorPortal.NFTs.Dtos
{
    public class NFTDto
    {
        public string TokenId { get; set; }
        public NFTMetadata Metadata { get; set; }
        public bool IsBurnable { get; set; }
        public bool IsTransferable { get; set; }
        public bool IsOnlyXRP { get; set; }
    }
}
