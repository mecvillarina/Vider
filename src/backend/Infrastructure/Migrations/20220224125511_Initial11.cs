using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForNewSubscriber",
                table: "CreatorAssets");

            migrationBuilder.AddColumn<int>(
                name: "Taxon",
                table: "CreatorAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CreatorAssets_Taxon",
                table: "CreatorAssets",
                column: "Taxon",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CreatorAssets_Taxon",
                table: "CreatorAssets");

            migrationBuilder.DropColumn(
                name: "Taxon",
                table: "CreatorAssets");

            migrationBuilder.AddColumn<bool>(
                name: "ForNewSubscriber",
                table: "CreatorAssets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
