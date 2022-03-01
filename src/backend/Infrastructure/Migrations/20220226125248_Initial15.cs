using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NftClaims_Creators_ReceiverId",
                table: "NftClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_NftClaims_Creators_SenderId",
                table: "NftClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NftIndexes",
                table: "NftIndexes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NftClaims",
                table: "NftClaims");

            migrationBuilder.RenameTable(
                name: "NftIndexes",
                newName: "NFTIndexes");

            migrationBuilder.RenameTable(
                name: "NftClaims",
                newName: "NFTClaims");

            migrationBuilder.RenameIndex(
                name: "IX_NftIndexes_UriHex",
                table: "NFTIndexes",
                newName: "IX_NFTIndexes_UriHex");

            migrationBuilder.RenameIndex(
                name: "IX_NftIndexes_Uri",
                table: "NFTIndexes",
                newName: "IX_NFTIndexes_Uri");

            migrationBuilder.RenameIndex(
                name: "IX_NftIndexes_TokenId",
                table: "NFTIndexes",
                newName: "IX_NFTIndexes_TokenId");

            migrationBuilder.RenameIndex(
                name: "IX_NftClaims_UriHex",
                table: "NFTClaims",
                newName: "IX_NFTClaims_UriHex");

            migrationBuilder.RenameIndex(
                name: "IX_NftClaims_Uri",
                table: "NFTClaims",
                newName: "IX_NFTClaims_Uri");

            migrationBuilder.RenameIndex(
                name: "IX_NftClaims_TokenId",
                table: "NFTClaims",
                newName: "IX_NFTClaims_TokenId");

            migrationBuilder.RenameIndex(
                name: "IX_NftClaims_SenderId",
                table: "NFTClaims",
                newName: "IX_NFTClaims_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_NftClaims_SellOfferIndex",
                table: "NFTClaims",
                newName: "IX_NFTClaims_SellOfferIndex");

            migrationBuilder.RenameIndex(
                name: "IX_NftClaims_ReceiverId",
                table: "NFTClaims",
                newName: "IX_NFTClaims_ReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFTIndexes",
                table: "NFTIndexes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFTClaims",
                table: "NFTClaims",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "NFTSellOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<int>(type: "int", nullable: true),
                    TokenId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UriHex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    TokenTaxon = table.Column<int>(type: "int", nullable: false),
                    SellOfferIndex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFTSellOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NFTSellOffers_Creators_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Creators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NFTSellOffers_SellerId",
                table: "NFTSellOffers",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_NFTSellOffers_SellOfferIndex",
                table: "NFTSellOffers",
                column: "SellOfferIndex");

            migrationBuilder.CreateIndex(
                name: "IX_NFTSellOffers_TokenId",
                table: "NFTSellOffers",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_NFTSellOffers_Uri",
                table: "NFTSellOffers",
                column: "Uri");

            migrationBuilder.CreateIndex(
                name: "IX_NFTSellOffers_UriHex",
                table: "NFTSellOffers",
                column: "UriHex");

            migrationBuilder.AddForeignKey(
                name: "FK_NFTClaims_Creators_ReceiverId",
                table: "NFTClaims",
                column: "ReceiverId",
                principalTable: "Creators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NFTClaims_Creators_SenderId",
                table: "NFTClaims",
                column: "SenderId",
                principalTable: "Creators",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFTClaims_Creators_ReceiverId",
                table: "NFTClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_NFTClaims_Creators_SenderId",
                table: "NFTClaims");

            migrationBuilder.DropTable(
                name: "NFTSellOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NFTIndexes",
                table: "NFTIndexes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NFTClaims",
                table: "NFTClaims");

            migrationBuilder.RenameTable(
                name: "NFTIndexes",
                newName: "NftIndexes");

            migrationBuilder.RenameTable(
                name: "NFTClaims",
                newName: "NftClaims");

            migrationBuilder.RenameIndex(
                name: "IX_NFTIndexes_UriHex",
                table: "NftIndexes",
                newName: "IX_NftIndexes_UriHex");

            migrationBuilder.RenameIndex(
                name: "IX_NFTIndexes_Uri",
                table: "NftIndexes",
                newName: "IX_NftIndexes_Uri");

            migrationBuilder.RenameIndex(
                name: "IX_NFTIndexes_TokenId",
                table: "NftIndexes",
                newName: "IX_NftIndexes_TokenId");

            migrationBuilder.RenameIndex(
                name: "IX_NFTClaims_UriHex",
                table: "NftClaims",
                newName: "IX_NftClaims_UriHex");

            migrationBuilder.RenameIndex(
                name: "IX_NFTClaims_Uri",
                table: "NftClaims",
                newName: "IX_NftClaims_Uri");

            migrationBuilder.RenameIndex(
                name: "IX_NFTClaims_TokenId",
                table: "NftClaims",
                newName: "IX_NftClaims_TokenId");

            migrationBuilder.RenameIndex(
                name: "IX_NFTClaims_SenderId",
                table: "NftClaims",
                newName: "IX_NftClaims_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_NFTClaims_SellOfferIndex",
                table: "NftClaims",
                newName: "IX_NftClaims_SellOfferIndex");

            migrationBuilder.RenameIndex(
                name: "IX_NFTClaims_ReceiverId",
                table: "NftClaims",
                newName: "IX_NftClaims_ReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NftIndexes",
                table: "NftIndexes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NftClaims",
                table: "NftClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NftClaims_Creators_ReceiverId",
                table: "NftClaims",
                column: "ReceiverId",
                principalTable: "Creators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NftClaims_Creators_SenderId",
                table: "NftClaims",
                column: "SenderId",
                principalTable: "Creators",
                principalColumn: "Id");
        }
    }
}
