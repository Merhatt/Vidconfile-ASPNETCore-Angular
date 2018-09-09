using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vidconfile.Data.Migrations
{
    public partial class VideoModelFixe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VidconfileUserVideo");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LikeCount",
                table: "Videos",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "VideoData",
                table: "Videos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Likes = table.Column<long>(nullable: false),
                    VideoId = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorId",
                table: "Comment",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_VideoId",
                table: "Comment",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoData",
                table: "Videos");

            migrationBuilder.CreateTable(
                name: "VidconfileUserVideo",
                columns: table => new
                {
                    VidconfileUserId = table.Column<Guid>(nullable: false),
                    VideoId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VidconfileUserVideo", x => new { x.VidconfileUserId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_VidconfileUserVideo_Users_VidconfileUserId",
                        column: x => x.VidconfileUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VidconfileUserVideo_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VidconfileUserVideo_VideoId",
                table: "VidconfileUserVideo",
                column: "VideoId");
        }
    }
}
