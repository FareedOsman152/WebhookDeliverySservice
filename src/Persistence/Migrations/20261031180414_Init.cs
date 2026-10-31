using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "marketplaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradeLicenseNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WebsiteLink = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shippers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradeLicenseNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WebsiteLink = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shippers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shippingContracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MarketplaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shippingContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shippingContracts_marketplaces_MarketplaceId",
                        column: x => x.MarketplaceId,
                        principalTable: "marketplaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shippingContracts_shippers_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "shippers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "marketplaceOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarketplaceOrderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplaceOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_marketplaceOrders_shippingContracts_ShippingContractId",
                        column: x => x.ShippingContractId,
                        principalTable: "shippingContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "marketplaceOrderEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusBefore = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusAfter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplaceOrderEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_marketplaceOrderEvents_marketplaceOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "marketplaceOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_marketplaceOrderEvents_OrderId",
                table: "marketplaceOrderEvents",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_marketplaceOrders_MarketplaceOrderId_ShippingContractId",
                table: "marketplaceOrders",
                columns: new[] { "MarketplaceOrderId", "ShippingContractId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marketplaceOrders_ShippingContractId",
                table: "marketplaceOrders",
                column: "ShippingContractId");

            migrationBuilder.CreateIndex(
                name: "IX_marketplaces_Email",
                table: "marketplaces",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marketplaces_PhoneNumber",
                table: "marketplaces",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marketplaces_TradeLicenseNumber",
                table: "marketplaces",
                column: "TradeLicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shippers_Email",
                table: "shippers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shippers_PhoneNumber",
                table: "shippers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shippers_TradeLicenseNumber",
                table: "shippers",
                column: "TradeLicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shippingContracts_MarketplaceId",
                table: "shippingContracts",
                column: "MarketplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_shippingContracts_ShipperId",
                table: "shippingContracts",
                column: "ShipperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "marketplaceOrderEvents");

            migrationBuilder.DropTable(
                name: "marketplaceOrders");

            migrationBuilder.DropTable(
                name: "shippingContracts");

            migrationBuilder.DropTable(
                name: "marketplaces");

            migrationBuilder.DropTable(
                name: "shippers");
        }
    }
}
