using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NftMetadataIndexes");

            migrationBuilder.CreateTable(
                name: "NftClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    ReceiverId = table.Column<int>(type: "int", nullable: true),
                    TokenId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UriHex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    TokenTaxon = table.Column<int>(type: "int", nullable: false),
                    SellOfferIndex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NftClaims_Creators_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Creators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NftClaims_Creators_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Creators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NftIndexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TokenTaxon = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UriHex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftIndexes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NftClaims_ReceiverId",
                table: "NftClaims",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_NftClaims_SellOfferIndex",
                table: "NftClaims",
                column: "SellOfferIndex");

            migrationBuilder.CreateIndex(
                name: "IX_NftClaims_SenderId",
                table: "NftClaims",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_NftClaims_TokenId",
                table: "NftClaims",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_NftClaims_Uri",
                table: "NftClaims",
                column: "Uri");

            migrationBuilder.CreateIndex(
                name: "IX_NftClaims_UriHex",
                table: "NftClaims",
                column: "UriHex");

            migrationBuilder.CreateIndex(
                name: "IX_NftIndexes_TokenId",
                table: "NftIndexes",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NftIndexes_Uri",
                table: "NftIndexes",
                column: "Uri",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NftIndexes_UriHex",
                table: "NftIndexes",
                column: "UriHex",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NftClaims");

            migrationBuilder.DropTable(
                name: "NftIndexes");

            migrationBuilder.CreateTable(
                name: "NftMetadataIndexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TokenId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TokenTaxon = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UriHex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftMetadataIndexes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NftMetadataIndexes_TokenId",
                table: "NftMetadataIndexes",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NftMetadataIndexes_Uri",
                table: "NftMetadataIndexes",
                column: "Uri",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NftMetadataIndexes_UriHex",
                table: "NftMetadataIndexes",
                column: "UriHex",
                unique: true);
        }
    }
}
