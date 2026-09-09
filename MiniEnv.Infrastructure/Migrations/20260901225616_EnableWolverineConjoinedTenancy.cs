using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniEnv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnableWolverineConjoinedTenancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_DealCustomers_Tenants_TenantId",
                table: "DealCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_DealObservers_Tenants_TenantId",
                table: "DealObservers");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Tenants_TenantId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Pipelines_Tenants_TenantId",
                table: "Pipelines");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Tenants_TenantId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Tenants_TenantId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Users",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                newName: "IX_Users_tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Stages",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Stages_TenantId",
                table: "Stages",
                newName: "IX_Stages_tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Roles",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "RolePermissions",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_TenantId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId_PermissionId_TenantId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId_PermissionId_tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Pipelines",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Pipelines_TenantId",
                table: "Pipelines",
                newName: "IX_Pipelines_tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Deals",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Deals_TenantId",
                table: "Deals",
                newName: "IX_Deals_tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "DealObservers",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_DealObservers_TenantId",
                table: "DealObservers",
                newName: "IX_DealObservers_tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "DealCustomers",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_DealCustomers_TenantId",
                table: "DealCustomers",
                newName: "IX_DealCustomers_tenant_id");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Customers",
                newName: "tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                newName: "IX_Customers_tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_PhoneNumber_TenantId",
                table: "Customers",
                newName: "IX_Customers_PhoneNumber_tenant_id");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Tenants",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "Stages",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "Roles",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "RolePermissions",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "Pipelines",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "Deals",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "DealObservers",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "DealCustomers",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "tenant_id",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "*DEFAULT*",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_tenant_id",
                table: "Roles",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tenants_tenant_id",
                table: "Customers",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DealCustomers_Tenants_tenant_id",
                table: "DealCustomers",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DealObservers_Tenants_tenant_id",
                table: "DealObservers",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Tenants_tenant_id",
                table: "Deals",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pipelines_Tenants_tenant_id",
                table: "Pipelines",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Tenants_tenant_id",
                table: "RolePermissions",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Tenants_tenant_id",
                table: "Stages",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_tenant_id",
                table: "Users",
                column: "tenant_id",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Tenants_tenant_id",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_DealCustomers_Tenants_tenant_id",
                table: "DealCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_DealObservers_Tenants_tenant_id",
                table: "DealObservers");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Tenants_tenant_id",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Pipelines_Tenants_tenant_id",
                table: "Pipelines");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Tenants_tenant_id",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Tenants_tenant_id",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_tenant_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Roles_tenant_id",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Users",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_tenant_id",
                table: "Users",
                newName: "IX_Users_TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Stages",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Stages_tenant_id",
                table: "Stages",
                newName: "IX_Stages_TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Roles",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "RolePermissions",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_tenant_id",
                table: "RolePermissions",
                newName: "IX_RolePermissions_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId_PermissionId_tenant_id",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId_PermissionId_TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Pipelines",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Pipelines_tenant_id",
                table: "Pipelines",
                newName: "IX_Pipelines_TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Deals",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Deals_tenant_id",
                table: "Deals",
                newName: "IX_Deals_TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "DealObservers",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_DealObservers_tenant_id",
                table: "DealObservers",
                newName: "IX_DealObservers_TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "DealCustomers",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_DealCustomers_tenant_id",
                table: "DealCustomers",
                newName: "IX_DealCustomers_TenantId");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Customers",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_tenant_id",
                table: "Customers",
                newName: "IX_Customers_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_PhoneNumber_tenant_id",
                table: "Customers",
                newName: "IX_Customers_PhoneNumber_TenantId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Users",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Tenants",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Stages",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Roles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "RolePermissions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Pipelines",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Deals",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "DealObservers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "DealCustomers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Customers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "*DEFAULT*");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DealCustomers_Tenants_TenantId",
                table: "DealCustomers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DealObservers_Tenants_TenantId",
                table: "DealObservers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Tenants_TenantId",
                table: "Deals",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pipelines_Tenants_TenantId",
                table: "Pipelines",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Tenants_TenantId",
                table: "RolePermissions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Tenants_TenantId",
                table: "Stages",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
