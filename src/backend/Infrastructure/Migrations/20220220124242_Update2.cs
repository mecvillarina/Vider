using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorSubscribers_Creators_SubsciberId",
                table: "CreatorSubscribers");

            migrationBuilder.RenameColumn(
                name: "SubsciberId",
                table: "CreatorSubscribers",
                newName: "SubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatorSubscribers_SubsciberId",
                table: "CreatorSubscribers",
                newName: "IX_CreatorSubscribers_SubscriberId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSubscribed",
                table: "CreatorSubscribers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorSubscribers_Creators_SubscriberId",
                table: "CreatorSubscribers",
                column: "SubscriberId",
                principalTable: "Creators",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorSubscribers_Creators_SubscriberId",
                table: "CreatorSubscribers");

            migrationBuilder.DropColumn(
                name: "DateSubscribed",
                table: "CreatorSubscribers");

            migrationBuilder.RenameColumn(
                name: "SubscriberId",
                table: "CreatorSubscribers",
                newName: "SubsciberId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatorSubscribers_SubscriberId",
                table: "CreatorSubscribers",
                newName: "IX_CreatorSubscribers_SubsciberId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorSubscribers_Creators_SubsciberId",
                table: "CreatorSubscribers",
                column: "SubsciberId",
                principalTable: "Creators",
                principalColumn: "Id");
        }
    }
}
