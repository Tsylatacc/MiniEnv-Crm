using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniEnv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDealCustomerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealCustomers_Customers_CustomerId",
                table: "DealCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Customers_CustomerId",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_CustomerId",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Deals");

            migrationBuilder.AddForeignKey(
                name: "FK_DealCustomers_Customers_CustomerId",
                table: "DealCustomers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealCustomers_Customers_CustomerId",
                table: "DealCustomers");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Deals",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CustomerId",
                table: "Deals",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealCustomers_Customers_CustomerId",
                table: "DealCustomers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Customers_CustomerId",
                table: "Deals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
