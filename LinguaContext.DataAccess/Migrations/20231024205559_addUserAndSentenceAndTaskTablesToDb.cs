using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinguaContext.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUserAndSentenceAndTaskTablesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserName = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    LastName = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: true),
                    BirthDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PersonalIntervalModifier = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Contexts",
                columns: table => new
                {
                    ContextId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contexts", x => x.ContextId);
                    table.ForeignKey(
                        name: "FK_Contexts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userSentenceInfos",
                columns: table => new
                {
                    UserSentenceInfoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SourceContextId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastEditedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PositionInText = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userSentenceInfos", x => x.UserSentenceInfoId);
                    table.ForeignKey(
                        name: "FK_userSentenceInfos_Contexts_SourceContextId",
                        column: x => x.SourceContextId,
                        principalTable: "Contexts",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userSentenceInfos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sentences",
                columns: table => new
                {
                    SentenceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Phrase = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Translation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Answer = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AnswerTranslation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserSentenceInfoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentences", x => x.SentenceId);
                    table.ForeignKey(
                        name: "FK_Sentences_userSentenceInfos_UserSentenceInfoId",
                        column: x => x.UserSentenceInfoId,
                        principalTable: "userSentenceInfos",
                        principalColumn: "UserSentenceInfoId");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SentenceId = table.Column<int>(type: "integer", nullable: false),
                    FirstTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextReview = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RepetitionNumber = table.Column<int>(type: "integer", nullable: false),
                    CurrentInterval = table.Column<int>(type: "integer", nullable: false)
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
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contexts_UserId",
                table: "Contexts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_UserSentenceInfoId",
                table: "Sentences",
                column: "UserSentenceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SentenceId",
                table: "Tasks",
                column: "SentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_userSentenceInfos_SourceContextId",
                table: "userSentenceInfos",
                column: "SourceContextId");

            migrationBuilder.CreateIndex(
                name: "IX_userSentenceInfos_UserId",
                table: "userSentenceInfos",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Sentences");

            migrationBuilder.DropTable(
                name: "userSentenceInfos");

            migrationBuilder.DropTable(
                name: "Contexts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
