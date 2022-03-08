using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TxHash",
                table: "ActivityLogs",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_TxHash",
                table: "ActivityLogs",
                column: "TxHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_TxHash",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "TxHash",
                table: "ActivityLogs");
        }
    }
}
