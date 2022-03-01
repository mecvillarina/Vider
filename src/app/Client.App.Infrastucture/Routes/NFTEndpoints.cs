namespace Client.App.Infrastructure.Routes
{
    public static class NFTEndpoints
    {
        public const string GetCreatorNFTs = "api/creatorportal/nfts/creatorNFTs?creatorId={0}";
        public const string GetNFTClaims = "api/creatorportal/nfts/claims";
        public const string GetNFTSellOffers = "api/creatorportal/nfts/selloffers?query={0}";
        public const string ClaimNFT = "api/creatorportal/nfts/claim";
        public const string BurnNFT = "api/creatorportal/nfts/burn";
        public const string GiftNFT = "api/creatorportal/nfts/gift";
        public const string MintNFT = "api/creatorportal/nfts/mint";
        public const string SellNFT = "api/creatorportal/nfts/sell";
        public const string CancelSellNFT = "api/creatorportal/nfts/cancelsell";
        public const string BuyNFT = "api/creatorportal/nfts/buy";

    }
}
