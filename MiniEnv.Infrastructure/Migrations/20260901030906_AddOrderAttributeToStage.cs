using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniEnv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAttributeToStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Stages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Outcome",
                table: "Stages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_Order_PipelineId",
                table: "Stages",
                columns: new[] { "Order", "PipelineId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stages_Order_PipelineId",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "Stages");
        }
    }
}
