namespace Domain.Entities
{
    public class NFTClaim
    {
        public int Id { get; set; }

        public int? SenderId { get; set; }
        public virtual Creator Sender { get; set; }

        public int? ReceiverId { get; set; }
        public virtual Creator Receiver { get; set; }

        public string TokenId { get; set; }
        public string Uri { get; set; }
        public string UriHex { get; set; }
        public int TokenTaxon { get; set; }
        public string SellOfferIndex { get; set; }
        public string Message { get; set; }
    }
}
