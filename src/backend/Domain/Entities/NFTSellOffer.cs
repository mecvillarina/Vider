using System;

namespace Domain.Entities
{
    public class NFTSellOffer
    {
        public int Id { get; set; }

        public int? SellerId { get; set; }
        public virtual Creator Seller { get; set; }

        public string TokenId { get; set; }
        public string Uri { get; set; }
        public string UriHex { get; set; }
        public int TokenTaxon { get; set; }
        public string SellOfferIndex { get; set; }
        public string Amount { get; set; }
        public bool IsExclusiveForSubscribers { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
