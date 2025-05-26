using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Nội thất phòng khách" },
                    { 2, "Nội thất phòng ngủ" },
                    { 3, "Nội thất nhà bếp" },
                    { 4, "Đồ trang trí" },
                    { 5, "Thiết bị thông minh" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImgUrl", "Price", "ProductName" },
                values: new object[,]
                {
                    { 1, 1, "Sofa 3 chỗ ngồi thiết kế hiện đại, chất liệu da cao cấp", "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400", 15000000m, "Sofa hiện đại" },
                    { 2, 1, "Bàn cà phê gỗ tự nhiên cao cấp, thiết kế tối giản", "https://images.unsplash.com/photo-1549497538-303791108f95?w=400", 3500000m, "Bàn cà phê gỗ" },
                    { 3, 2, "Giường ngủ king size với đầu giường bọc da, khung gỗ sồi", "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?w=400", 12000000m, "Giường ngủ king size" },
                    { 4, 2, "Tủ quần áo 4 cánh gỗ công nghiệp, có gương và ngăn kéo", "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=400", 8500000m, "Tủ quần áo" },
                    { 5, 3, "Bộ bàn ăn 6 ghế gỗ sồi, thiết kế sang trọng", "https://images.unsplash.com/photo-1449247709967-d4461a6a6103?w=400", 9500000m, "Bộ bàn ăn" },
                    { 6, 1, "Kệ tivi gỗ công nghiệp, có ngăn chứa đồ", "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400", 4200000m, "Kệ tivi hiện đại" },
                    { 7, 3, "Tủ bếp gỗ công nghiệp chống ẩm, mặt đá granite", "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400", 25000000m, "Tủ bếp cao cấp" },
                    { 8, 4, "Đèn chùm pha lê cao cấp, ánh sáng LED", "https://images.unsplash.com/photo-1524484485831-a92ffc0de03f?w=400", 1800000m, "Đèn trang trí" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
