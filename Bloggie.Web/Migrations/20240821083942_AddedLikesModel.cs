using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedLikesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPostLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostLikes_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1f7b8bac-21ec-4dab-8554-8722caf69cc4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "23ed88fa-48b9-4bac-a29d-62ae5b506722", "AQAAAAIAAYagAAAAEO6zkS3iVOL4SSmut0UBqIqd6ZbOXAl/iwHDvYBu8DOoxaRWsmr7vNtANgWZrnLpSQ==", "4355042e-3699-4581-b252-1e55ff102c06" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostLikes_BlogPostId",
                table: "BlogPostLikes",
                column: "BlogPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostLikes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1f7b8bac-21ec-4dab-8554-8722caf69cc4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "568260d0-eac4-4b33-83f5-8c928f717000", "AQAAAAIAAYagAAAAEJWksZaBnQvGIza+BBJhXYjQhrnn1x7Na17c2+sRctfxiepj0EYt7d1YinDUbJsMXA==", "929c9a7b-a5ba-4d91-b9fe-d46c690dcc94" });
        }
    }
}
