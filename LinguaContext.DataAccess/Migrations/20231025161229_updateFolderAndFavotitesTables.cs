using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguaContext.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateFolderAndFavotitesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteSentences",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SentenceId = table.Column<int>(type: "integer", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteSentences", x => new { x.UserId, x.SentenceId });
                });

            migrationBuilder.CreateTable(
                name: "SentencesInFolder",
                columns: table => new
                {
                    FolderId = table.Column<int>(type: "integer", nullable: false),
                    SentenceId = table.Column<int>(type: "integer", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentencesInFolder", x => new { x.FolderId, x.SentenceId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteSentences");

            migrationBuilder.DropTable(
                name: "SentencesInFolder");
        }
    }
}
