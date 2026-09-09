using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniEnv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenToSignUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "SignUps",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "SignUps");
        }
    }
}
