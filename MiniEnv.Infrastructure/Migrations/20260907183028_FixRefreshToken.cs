using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniEnv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetTokens_Users_UserId1",
                table: "PasswordResetTokens");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetTokens_UserId1",
                table: "PasswordResetTokens");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PasswordResetTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "PasswordResetTokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_UserId1",
                table: "PasswordResetTokens",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetTokens_Users_UserId1",
                table: "PasswordResetTokens",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
