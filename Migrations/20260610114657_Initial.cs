using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Organi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CaloriesPer100g = table.Column<int>(type: "int", nullable: true),
                    FatPer100g = table.Column<double>(type: "float", nullable: true),
                    CarbsPer100g = table.Column<double>(type: "float", nullable: true),
                    ProteinPer100g = table.Column<double>(type: "float", nullable: true),
                    ServingSize = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Badge", "CaloriesPer100g", "CarbsPer100g", "Category", "CreatedAt", "Description", "FatPer100g", "ImageUrl", "IsActive", "IsFeatured", "Name", "OldPrice", "Price", "ProteinPer100g", "Rating", "RatingCount", "ServingSize", "Stock", "Unit", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "İNDİRİM", 31, 6.0, 1, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7214), "Çiftliğimizden taze organik biber.", 0.29999999999999999, "/images/placeholder.png", true, false, "Organik Biber", 3.49m, 2.29m, 1.0, 4.5, 15, "100g", 50, "kg", null },
                    { 2, null, 550, 45.0, 5, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7235), "Doğal bitter çikolata.", 35.0, "/images/placeholder.png", true, false, "Organik Bitter Çikolata", null, 4.49m, 5.0, 4.5, 14, "30g", 30, "paket", null },
                    { 3, null, 342, 71.0, 5, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7239), "Tam buğday bulguru.", 1.3, "/images/placeholder.png", true, false, "Organik Bulgur", null, 2.49m, 12.0, 4.0, 8, "50g", 100, "paket", null },
                    { 4, "HOT", 654, 14.0, 5, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7242), "Organik iç ceviz 200g.", 65.200000000000003, "/images/placeholder.png", true, false, "Organik Ceviz", null, 9.99m, 15.199999999999999, 4.9000000000000004, 91, "30g porsiyon", 45, "paket", null },
                    { 5, "İNDİRİM", 32, 7.7000000000000002, 2, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7245), "Taze organik çilek.", 0.29999999999999999, "/images/placeholder.png", true, false, "Organik Çilek", 5.99m, 4.59m, 0.69999999999999996, 4.5, 30, "100g", 25, "paket", null },
                    { 6, null, 52, 14.0, 2, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7249), "Doğal organik elma.", 0.20000000000000001, "/images/placeholder.png", true, false, "Organik Elma", null, 2.99m, 0.29999999999999999, 4.5, 22, "1 adet (150g)", 80, "kg", null },
                    { 7, null, 31, 7.0, 1, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7251), "Taze organik yeşil fasulye.", 0.20000000000000001, "/images/placeholder.png", true, false, "Organik Fasulye", null, 2.29m, 1.8, 3.5, 12, "100g", 60, "kg", null },
                    { 8, null, 607, 16.0, 5, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7254), "Doğal fındık karışımı.", 54.0, "/images/placeholder.png", true, false, "Organik Fındık Karışımı", null, 8.99m, 15.0, 4.5, 18, "30g", 40, "paket", null },
                    { 9, null, 430, 57.0, 5, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7256), "Organik doğal granola bar 40g.", 15.0, "/images/placeholder.png", true, false, "Organik Granola Bar", null, 3.99m, 8.4000000000000004, 4.4000000000000004, 27, "1 bar (45g)", 100, "paket", null },
                    { 10, null, 41, 10.0, 1, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7259), "Taze organik havuç.", 0.20000000000000001, "/images/placeholder.png", true, false, "Organik Havuç", 2.19m, 1.49m, 0.90000000000000002, 4.0, 19, "100g", 70, "kg", null },
                    { 11, null, 241, 63.0, 5, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7263), "Doğal kuru kayısı.", 0.5, "/images/placeholder.png", true, false, "Organik Kuru Kayısı", null, 5.49m, 3.3999999999999999, 4.5, 33, "30g", 55, "paket", null },
                    { 12, null, 61, 4.7999999999999998, 3, new DateTime(2026, 6, 10, 14, 46, 56, 600, DateTimeKind.Local).AddTicks(7265), "Taze organik süt 1 litre.", 3.2000000000000002, "/images/placeholder.png", true, true, "Organik Süt", null, 1.99m, 3.2000000000000002, 4.7999999999999998, 45, "200ml", 90, "litre", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
