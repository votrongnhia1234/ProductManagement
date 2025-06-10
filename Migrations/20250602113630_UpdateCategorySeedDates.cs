using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategorySeedDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShippingAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "Manager", "MANAGER" },
                    { "3", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(8935), "Thiết bị và tiện ích điện tử", "Điện tử", new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9074) },
                    { 2, new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9195), "Nhiều loại sách và tiểu thuyết", "Sách", new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9195) },
                    { 3, new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9197), "Quần áo và các mặt hàng thời trang", "Thời trang", new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9197) },
                    { 4, new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9198), "Đồ dùng cho nhà cửa và sân vườn", "Nhà cửa & Vườn", new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9199) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImgUrl", "Price", "ProductName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy tính xách tay mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 1200.00m, "Máy tính xách tay", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Điện thoại thông minh mô tả chi tiết", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9", 800.00m, "Điện thoại thông minh", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 2, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chúa tể những chiếc nhẫn mô tả chi tiết", "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f", 20.00m, "Chúa tể những chiếc nhẫn", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 2, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Xứ Cát mô tả chi tiết", "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f", 15.00m, "Xứ Cát", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 3, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo phông mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 25.00m, "Áo phông", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 3, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quần jean mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 50.00m, "Quần jean", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy ảnh Kỹ thuật số mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 650.00m, "Máy ảnh Kỹ thuật số", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tai nghe Bluetooth mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 150.00m, "Tai nghe Bluetooth", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 1, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Màn hình máy tính mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 300.00m, "Màn hình máy tính", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bàn phím cơ mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 100.00m, "Bàn phím cơ", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 1, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuột không dây mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 25.00m, "Chuột không dây", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 2, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tiểu thuyết trinh thám mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 18.00m, "Tiểu thuyết trinh thám", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 2, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sách nấu ăn mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 30.00m, "Sách nấu ăn", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 2, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lịch sử Việt Nam mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 25.00m, "Lịch sử Việt Nam", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 2, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Truyện tranh mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 10.00m, "Truyện tranh", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 3, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy đầm mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 40.00m, "Váy đầm", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 3, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo sơ mi mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 35.00m, "Áo sơ mi", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 3, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giày thể thao mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 70.00m, "Giày thể thao", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 4, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bàn làm việc mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 150.00m, "Bàn làm việc", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 4, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ghế văn phòng mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 80.00m, "Ghế văn phòng", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 4, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đèn bàn mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 30.00m, "Đèn bàn", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 4, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cây cảnh mini mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8", 10.00m, "Cây cảnh mini", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
