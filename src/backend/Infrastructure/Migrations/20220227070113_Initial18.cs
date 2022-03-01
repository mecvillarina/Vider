using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostedById = table.Column<int>(type: "int", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedPosts_Creators_PostedById",
                        column: x => x.PostedById,
                        principalTable: "Creators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedPostLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    LikedById = table.Column<int>(type: "int", nullable: true),
                    DateOccured = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedPostLikes_Creators_LikedById",
                        column: x => x.LikedById,
                        principalTable: "Creators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeedPostLikes_FeedPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "FeedPosts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedPostLikes_LikedById",
                table: "FeedPostLikes",
                column: "LikedById");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPostLikes_PostId",
                table: "FeedPostLikes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPosts_PostedById",
                table: "FeedPosts",
                column: "PostedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedPostLikes");

            migrationBuilder.DropTable(
                name: "FeedPosts");
        }
    }
}
