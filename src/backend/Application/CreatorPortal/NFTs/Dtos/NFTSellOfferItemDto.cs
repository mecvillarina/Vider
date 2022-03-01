using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.CreatorPortal.NFTs.Dtos
{
    public class NFTSellOfferItemDto : IMapFrom<NFTSellOfferItem>
    {
        public int SellerId { get; set; }
        public string SellerUsername { get; set; }
        public string SellerProfilePictureLink { get; set; }

        public NFTDto NFT { get; set; }

        public int SellOfferId { get; set; }
        public double SellOfferAmount { get; set; }
        public DateTime SellOfferDatePosted { get; set; }
        public bool SellOfferIsExclusiveForSubscribers { get; set; }
        public bool CanBuy { get; set; }
    }
}
