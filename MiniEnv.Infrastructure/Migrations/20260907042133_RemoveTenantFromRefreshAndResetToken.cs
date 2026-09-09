using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniEnv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTenantFromRefreshAndResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetTokens_Tenants_tenant_id",
                table: "PasswordResetTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Tenants_tenant_id",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_tenant_id",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetTokens_tenant_id",
                table: "PasswordResetTokens");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "PasswordResetTokens");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "tenant_id",
                table: "RefreshTokens",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*");

            migrationBuilder.AddColumn<string>(
                name: "tenant_id",
                table: "PasswordResetTokens",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_tenant_id",
                table: "RefreshTokens",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_tenant_id",
                table: "PasswordResetTokens",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetTokens_Tenants_tenant_id",
                table: "PasswordResetTokens",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Tenants_tenant_id",
                table: "RefreshTokens",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
