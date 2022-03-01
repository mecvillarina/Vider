using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NFTSellOfferView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW NFTSellOfferView as
                    SELECT T3.Id as SellerId, T3.Username as SellerUsername, T3.Name as SellerName, T3.ProfilePictureFilename as SellerProfilePictureFilename, T2.Uri as NFTUri, T2.UriHex as NFTUriHex, T2.TokenId as NFTTokenId, T2.Metadata as NFTMetadata, T1.Id as SellOfferId, T1.Amount, T1.SellOfferIndex, T1.DatePosted as DatePosted, T1.IsExclusiveForSubscribers as IsExclusiveForSubscribers FROM NFTSellOffers T1
                    JOIN NFTIndexes T2 ON T1.TokenId = T2.TokenId AND T1.UriHex = T2.UriHex
                    JOIN Creators T3 ON T3.Id = T1.SellerId
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                drop view NFTSellOfferView;
            ");
        }
    }
}
