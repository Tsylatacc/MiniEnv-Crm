using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniEnv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTenantAttributeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Tenants",
                newName: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Tenants",
                newName: "UpdateAt");
        }
    }
}
