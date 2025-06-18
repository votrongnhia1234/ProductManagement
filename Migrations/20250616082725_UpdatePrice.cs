using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrice : Migration
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
                    { 1, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop, PC, máy tính bảng và phụ kiện", "Laptop & Máy tính", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smartphone, tablet và phụ kiện di động", "Điện thoại & Tablet", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tai nghe, loa, thiết bị âm thanh", "Âm thanh & Tai nghe", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết bị gaming, console, phụ kiện game", "Gaming & Esports", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy ảnh, camera, thiết bị quay phim", "Camera & Quay phim", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Router, modem, thiết bị mạng và WiFi", "Thiết bị mạng", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bàn phím, chuột, màn hình, webcam", "Phụ kiện máy tính", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smart home, IoT, thiết bị tự động hóa", "Thiết bị thông minh", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "CPU, RAM, ổ cứng, card đồ họa", "Linh kiện máy tính", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ốp lưng, sạc, cáp, pin dự phòng", "Phụ kiện di động", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy in, scanner, máy chiếu, thiết bị VP", "Thiết bị văn phòng", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smartwatch, fitness tracker, wearable", "Đồng hồ thông minh", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImgUrl", "Price", "ProductName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop Apple M3 chip, 16GB RAM, 512GB SSD", "/images/products/macbook-pro-m3.jpg", 4500.00m, "MacBook Pro M3 14 inch", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop Dell Intel Core i7, 16GB RAM, 1TB SSD", "/images/products/dell-xps-13.jpg", 3500.00m, "Dell XPS 13 Plus", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaming laptop AMD Ryzen 7, RTX 4060, 16GB RAM", "/images/products/asus-rog-strix.jpg", 2800.00m, "ASUS ROG Strix G15", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy tính bảng Apple M2, 256GB, WiFi + Cellular", "/images/products/ipad-pro-m2.jpg", 32.00m, "iPad Pro 12.9 inch M2", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 2, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "iPhone 15 Pro Max 256GB, Titanium Natural", "/images/products/iphone-15-pro-max.jpg", 3400.00m, "iPhone 15 Pro Max", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 2, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galaxy S24 Ultra 512GB, S Pen, AI Camera", "/images/products/galaxy-s24-ultra.jpg", 3100.00m, "Samsung Galaxy S24 Ultra", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 2, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pixel 8 Pro 256GB, AI Photography, Pure Android", "/images/products/pixel-8-pro.jpg", 2500.00m, "Google Pixel 8 Pro", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Xiaomi 14 Ultra 512GB, Leica Camera, Snapdragon 8 Gen 3", "/images/products/xiaomi-14-ultra.jpg", 2200.00m, "Xiaomi 14 Ultra", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 3, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tai nghe Apple AirPods Pro, Active Noise Cancelling", "/images/products/airpods-pro-2.jpg", 6500.00m, "AirPods Pro 2nd Gen", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 3, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tai nghe over-ear Sony, chống ồn hàng đầu", "/images/products/sony-wh1000xm5.jpg", 8500.00m, "Sony WH-1000XM5", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 3, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loa Bluetooth JBL chống nước, bass mạnh", "/images/products/jbl-charge-5.jpg", 3500.00m, "JBL Charge 5", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 3, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tai nghe Bose chống ồn, âm thanh Hi-Fi", "/images/products/bose-qc45.jpg", 7500.00m, "Bose QuietComfort 45", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 4, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Console PS5 Slim 1TB, 4K Gaming, Ray Tracing", "/images/products/ps5-slim.jpg", 14000.00m, "PlayStation 5 Slim", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 4, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Console Xbox Series X 1TB, 4K 120fps Gaming", "/images/products/xbox-series-x.jpg", 13500.00m, "Xbox Series X", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 4, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuột gaming Razer wireless, sensor Focus Pro 30K", "/images/products/razer-deathadder-v3.jpg", 3500.00m, "Razer DeathAdder V3 Pro", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 4, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bàn phím cơ gaming, OmniPoint switches", "/images/products/steelseries-apex-pro.jpg", 4500.00m, "SteelSeries Apex Pro TKL", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 5, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy ảnh mirrorless Canon, 24.2MP, 4K Video", "/images/products/canon-r6-mark2.jpg", 4200.00m, "Canon EOS R6 Mark II", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 5, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy ảnh Sony Alpha 33MP, 4K 60p, Real-time Eye AF", "/images/products/sony-a7-iv.jpg", 4800.00m, "Sony A7 IV", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 5, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drone DJI 4K HDR, 3-axis gimbal, 45 phút bay", "/images/products/dji-mini-4-pro.jpg", 1800.00m, "DJI Mini 4 Pro", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 5, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Action camera GoPro 5.3K, HyperSmooth 6.0", "/images/products/gopro-hero-12.jpg", 1200.00m, "GoPro Hero 12 Black", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 6, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Router ASUS WiFi 6E, 6000Mbps, Mesh ready", "/images/products/asus-ax6000.jpg", 8000.00m, "ASUS AX6000 WiFi 6E", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 6, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Router TP-Link WiFi 6, 5400Mbps, OneMesh", "/images/products/tplink-ax73.jpg", 3000.00m, "TP-Link Archer AX73", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 6, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hệ thống Mesh WiFi 6, phủ sóng 500m²", "/images/products/netgear-orbi.jpg", 1200.00m, "Netgear Orbi AX6000", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 7, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Màn hình cong 34 inch, 3440x1440, USB-C", "/images/products/lg-ultrawide-34.jpg", 8000.00m, "LG UltraWide 34WP65C", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 7, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuột wireless Logitech, 8000 DPI, USB-C", "/images/products/logitech-mx-master-3s.jpg", 2000.00m, "Logitech MX Master 3S", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 7, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bàn phím cơ wireless, hot-swap, RGB", "/images/products/keychron-k8-pro.jpg", 3000.00m, "Keychron K8 Pro", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 7, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Webcam Logitech 1080p, auto-focus, stereo audio", "/images/products/logitech-c920s.jpg", 1000.00m, "Logitech C920s Pro HD", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 8, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loa thông minh Amazon Alexa, điều khiển giọng nói", "/images/products/echo-dot-5.jpg", 100.00m, "Amazon Echo Dot 5th Gen", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, 8, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Màn hình thông minh Google 10 inch, camera", "/images/products/nest-hub-max.jpg", 6500.00m, "Google Nest Hub Max", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 8, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bộ đèn LED thông minh Philips, điều khiển màu sắc", "/images/products/philips-hue.jpg", 2800.00m, "Philips Hue White Ambiance", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 9, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "CPU AMD 12 cores, 24 threads, 5.6GHz boost", "/images/products/amd-ryzen-9-7900x.jpg", 1200.00m, "AMD Ryzen 9 7900X", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 9, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card đồ họa NVIDIA 12GB GDDR6X, DLSS 3", "/images/products/rtx-4070-super.jpg", 1800.00m, "NVIDIA RTX 4070 Super", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 9, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "RAM Corsair 32GB (2x16GB) DDR5-5600, RGB", "/images/products/corsair-vengeance-ddr5.jpg", 4000.00m, "Corsair Vengeance DDR5", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 9, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "SSD NVMe Samsung 2TB, 7000MB/s read speed", "/images/products/samsung-980-pro.jpg", 5000.00m, "Samsung 980 PRO 2TB", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 10, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pin sạc dự phòng Anker 26800mAh, sạc nhanh PD", "/images/products/anker-powercore.jpg", 1000.00m, "Anker PowerCore 26800", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 10, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đế sạc không dây Belkin cho iPhone, AirPods, Apple Watch", "/images/products/belkin-magsafe-3in1.jpg", 3000.00m, "Belkin MagSafe 3-in-1", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 10, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chân máy Peak Design cho smartphone, gấp gọn", "/images/products/peak-design-tripod.jpg", 1000.00m, "Peak Design Mobile Tripod", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 11, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy in laser HP đen trắng, tốc độ 38 trang/phút", "/images/products/hp-laserjet-m404n.jpg", 4500.00m, "HP LaserJet Pro M404n", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 11, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy in phun màu Epson, in scan copy, WiFi", "/images/products/epson-l3250.jpg", 3000.00m, "Epson EcoTank L3250", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 11, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy chiếu BenQ WXGA 4000 lumens, HDMI", "/images/products/benq-mw560.jpg", 1200.00m, "BenQ MW560 Projector", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, 12, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apple Watch 45mm, GPS + Cellular, Always-On display", "/images/products/apple-watch-series-9.jpg", 1200.00m, "Apple Watch Series 9", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, 12, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galaxy Watch 6 44mm, Wear OS, health tracking", "/images/products/galaxy-watch-6.jpg", 8500.00m, "Samsung Galaxy Watch 6", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 12, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đồng hồ thể thao Garmin, GPS, pin năng lượng mặt trời", "/images/products/garmin-fenix-7x.jpg", 1800.00m, "Garmin Fenix 7X Solar", new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
