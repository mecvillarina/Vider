using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UsernameNormalize = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccountXAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccountSecret = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccountClassicAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccountAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsAccountValid = table.Column<bool>(type: "bit", nullable: false),
                    DateAccountAcquired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreatorPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Digest = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatorPasswords_Creators_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Creators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatorSubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    SubsciberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorSubscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatorSubscribers_Creators_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Creators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatorSubscribers_Creators_SubsciberId",
                        column: x => x.SubsciberId,
                        principalTable: "Creators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreatorPasswords_CreatorId",
                table: "CreatorPasswords",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Creators_Username",
                table: "Creators",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Creators_UsernameNormalize",
                table: "Creators",
                column: "UsernameNormalize",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatorSubscribers_CreatorId",
                table: "CreatorSubscribers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorSubscribers_SubsciberId",
                table: "CreatorSubscribers",
                column: "SubsciberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreatorPasswords");

            migrationBuilder.DropTable(
                name: "CreatorSubscribers");

            migrationBuilder.DropTable(
                name: "Creators");
        }
    }
}
