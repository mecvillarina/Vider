using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class NFTSellOfferItemConfiguration : IEntityTypeConfiguration<NFTSellOfferItem>
    {
        public void Configure(EntityTypeBuilder<NFTSellOfferItem> builder)
        {
            builder.ToView("NFTSellOfferView")
                .HasKey(t => t.SellOfferId);

            //migrationBuilder.Sql(@"
            //    CREATE VIEW NFTSellOfferView as
            //SELECT T3.Id as SellerId, T3.Username as SellerUsername, T3.Name as SellerName, T3.ProfilePictureFilename as SellerProfilePictureFilename, T3.IsAccountValid as SellerAccountValid, T2.Uri as NFTUri, T2.UriHex as NFTUriHex, T2.TokenId as NFTTokenId, T2.Metadata as NFTMetadata,T2.TokenFlags as NFTTokenFlags, T2.NftSerial as NFTSerial, T1.Id as SellOfferId, T1.Amount, T1.SellOfferIndex, T1.DatePosted as DatePosted, T1.IsExclusiveForSubscribers as IsExclusiveForSubscribers FROM NFTSellOffers T1
            //  JOIN NFTIndexes T2 ON T1.TokenId = T2.TokenId AND T1.UriHex = T2.UriHex
            //JOIN Creators T3 ON T3.Id = T1.SellerId
            //    ");
            //migrationBuilder.Sql(@"
            //    drop view NFTSellOfferView;
            //");
        }
    }
}
