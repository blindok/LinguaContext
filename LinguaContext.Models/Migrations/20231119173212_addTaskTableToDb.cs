using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguaContext.Models.Migrations
{
    /// <inheritdoc />
    public partial class addTaskTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SentenceId = table.Column<int>(type: "integer", nullable: false),
                    FirstReview = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastReview = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextReview = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RepetitionNumber = table.Column<int>(type: "integer", nullable: false),
                    CurrentInterval = table.Column<int>(type: "integer", nullable: false),
                    EaseFactor = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => new { x.UserId, x.SentenceId });
                    table.ForeignKey(
                        name: "FK_Tasks_Sentences_SentenceId",
                        column: x => x.SentenceId,
                        principalTable: "Sentences",
                        principalColumn: "SentenceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SentenceId",
                table: "Tasks",
                column: "SentenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
