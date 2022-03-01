using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NftMetadataIndexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TokenTaxon = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UriHex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NftMetadataIndexes");
        }
    }
}
