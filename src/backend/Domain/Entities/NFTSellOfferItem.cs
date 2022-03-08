using System;

namespace Domain.Entities
{
    public class NFTSellOfferItem
    {
        public int SellerId { get; set; }
        public string SellerAccountAddress { get; set; }
        public string SellerUsername { get; set; }
        public bool SellerAccountValid { get; set; }
        public bool SellerIsAdmin { get; set; }

        public string SellerProfilePictureFilename { get; set; }
        public string NFTUri { get; set; }
        public string NFTUriHex { get; set; }
        public string NFTTokenId { get; set; }
        public string NFTMetadata { get; set; }
        public int NFTTokenFlags { get; set; }
        public int NFTSerial { get; set; }
        public int SellOfferId { get; set; }
        public string Amount { get; set; }
        public string SellOfferIndex { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsExclusiveForSubscribers { get; set; }
    }
}
