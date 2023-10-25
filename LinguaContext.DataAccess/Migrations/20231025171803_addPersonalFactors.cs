using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinguaContext.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addPersonalFactors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sentences_userSentenceInfos_UserSentenceInfoId",
                table: "Sentences");

            migrationBuilder.DropForeignKey(
                name: "FK_userSentenceInfos_Contexts_SourceContextId",
                table: "userSentenceInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_userSentenceInfos_Users_UserId",
                table: "userSentenceInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userSentenceInfos",
                table: "userSentenceInfos");

            migrationBuilder.DropIndex(
                name: "IX_Sentences_UserSentenceInfoId",
                table: "Sentences");

            migrationBuilder.DropColumn(
                name: "PersonalIntervalModifier",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "userSentenceInfos",
                newName: "UserSentenceInfos");

            migrationBuilder.RenameIndex(
                name: "IX_userSentenceInfos_UserId",
                table: "UserSentenceInfos",
                newName: "IX_UserSentenceInfos_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_userSentenceInfos_SourceContextId",
                table: "UserSentenceInfos",
                newName: "IX_UserSentenceInfos_SourceContextId");

            migrationBuilder.RenameColumn(
                name: "LastTime",
                table: "Tasks",
                newName: "LastReview");

            migrationBuilder.RenameColumn(
                name: "FirstTime",
                table: "Tasks",
                newName: "FirstReview");

            migrationBuilder.AddColumn<double>(
                name: "EaseFactor",
                table: "Tasks",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSentenceInfos",
                table: "UserSentenceInfos",
                column: "UserSentenceInfoId");

            migrationBuilder.CreateTable(
                name: "PersonalFactors",
                columns: table => new
                {
                    PersonalFactorsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IntervalModifier = table.Column<double>(type: "double precision", nullable: false),
                    FailIntervalModifier = table.Column<double>(type: "double precision", nullable: false),
                    HardIntervalModifier = table.Column<double>(type: "double precision", nullable: false),
                    EasyIntervalModifier = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalFactors", x => x.PersonalFactorsId);
                    table.ForeignKey(
                        name: "FK_PersonalFactors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_UserSentenceInfoId",
                table: "Sentences",
                column: "UserSentenceInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFactors_UserId",
                table: "PersonalFactors",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sentences_UserSentenceInfos_UserSentenceInfoId",
                table: "Sentences",
                column: "UserSentenceInfoId",
                principalTable: "UserSentenceInfos",
                principalColumn: "UserSentenceInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSentenceInfos_Contexts_SourceContextId",
                table: "UserSentenceInfos",
                column: "SourceContextId",
                principalTable: "Contexts",
                principalColumn: "ContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSentenceInfos_Users_UserId",
                table: "UserSentenceInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sentences_UserSentenceInfos_UserSentenceInfoId",
                table: "Sentences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSentenceInfos_Contexts_SourceContextId",
                table: "UserSentenceInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSentenceInfos_Users_UserId",
                table: "UserSentenceInfos");

            migrationBuilder.DropTable(
                name: "PersonalFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSentenceInfos",
                table: "UserSentenceInfos");

            migrationBuilder.DropIndex(
                name: "IX_Sentences_UserSentenceInfoId",
                table: "Sentences");

            migrationBuilder.DropColumn(
                name: "EaseFactor",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "UserSentenceInfos",
                newName: "userSentenceInfos");

            migrationBuilder.RenameIndex(
                name: "IX_UserSentenceInfos_UserId",
                table: "userSentenceInfos",
                newName: "IX_userSentenceInfos_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSentenceInfos_SourceContextId",
                table: "userSentenceInfos",
                newName: "IX_userSentenceInfos_SourceContextId");

            migrationBuilder.RenameColumn(
                name: "LastReview",
                table: "Tasks",
                newName: "LastTime");

            migrationBuilder.RenameColumn(
                name: "FirstReview",
                table: "Tasks",
                newName: "FirstTime");

            migrationBuilder.AddColumn<double>(
                name: "PersonalIntervalModifier",
                table: "Users",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userSentenceInfos",
                table: "userSentenceInfos",
                column: "UserSentenceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_UserSentenceInfoId",
                table: "Sentences",
                column: "UserSentenceInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sentences_userSentenceInfos_UserSentenceInfoId",
                table: "Sentences",
                column: "UserSentenceInfoId",
                principalTable: "userSentenceInfos",
                principalColumn: "UserSentenceInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_userSentenceInfos_Contexts_SourceContextId",
                table: "userSentenceInfos",
                column: "SourceContextId",
                principalTable: "Contexts",
                principalColumn: "ContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_userSentenceInfos_Users_UserId",
                table: "userSentenceInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
