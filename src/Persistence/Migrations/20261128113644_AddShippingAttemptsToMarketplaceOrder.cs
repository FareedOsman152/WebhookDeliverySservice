using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingAttemptsToMarketplaceOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingAttempts",
                table: "marketplaceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_marketplaceOrders_ShippingAttempts",
                table: "marketplaceOrders",
                sql: "[ShippingAttempts] BETWEEN 0 AND 10");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_marketplaceOrders_ShippingAttempts",
                table: "marketplaceOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAttempts",
                table: "marketplaceOrders");
        }
    }
}
