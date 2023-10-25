using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinguaContext.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addFolderAndFavoritesTablesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userSentenceInfos_Contexts_SourceContextId",
                table: "userSentenceInfos");

            migrationBuilder.AlterColumn<int>(
                name: "SourceContextId",
                table: "userSentenceInfos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "PositionInText",
                table: "userSentenceInfos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDay",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    FolderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FolderName = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SharingStatusCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.FolderId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_userSentenceInfos_Contexts_SourceContextId",
                table: "userSentenceInfos",
                column: "SourceContextId",
                principalTable: "Contexts",
                principalColumn: "ContextId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userSentenceInfos_Contexts_SourceContextId",
                table: "userSentenceInfos");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.AlterColumn<int>(
                name: "SourceContextId",
                table: "userSentenceInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionInText",
                table: "userSentenceInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_userSentenceInfos_Contexts_SourceContextId",
                table: "userSentenceInfos",
                column: "SourceContextId",
                principalTable: "Contexts",
                principalColumn: "ContextId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
