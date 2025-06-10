using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataToJune2025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Máy tính xách tay hiệu năng cao", "https://via.placeholder.com/300x200?text=Laptop" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Điện thoại thông minh mẫu mới nhất", "https://via.placeholder.com/300x200?text=Smartphone" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Tiểu thuyết giả tưởng sử thi", "https://via.placeholder.com/300x200?text=Book" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Khoa học viễn tưởng kinh điển", "https://via.placeholder.com/300x200?text=Book" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Áo phông cotton thoải mái", "https://via.placeholder.com/300x200?text=T-Shirt" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Quần jean vải bền", "https://via.placeholder.com/300x200?text=Jeans" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Máy ảnh độ phân giải cao", "https://via.placeholder.com/300x200?text=Camera" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Tai nghe không dây chống ồn", "https://via.placeholder.com/300x200?text=Headphones" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Màn hình Full HD 27 inch", "https://via.placeholder.com/300x200?text=Monitor" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Bàn phím cơ với đèn nền RGB", "https://via.placeholder.com/300x200?text=Keyboard" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Chuột quang không dây", "https://via.placeholder.com/300x200?text=Mouse" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Câu chuyện hấp dẫn đầy bí ẩn", "https://via.placeholder.com/300x200?text=Mystery+Book" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Tuyển tập công thức món ngon", "https://via.placeholder.com/300x200?text=Cookbook" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Tìm hiểu về lịch sử hào hùng", "https://via.placeholder.com/300x200?text=History+Book" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Bộ sưu tập truyện tranh vui nhộn", "https://via.placeholder.com/300x200?text=Comic+Book" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Váy thời trang cho nữ", "https://via.placeholder.com/300x200?text=Dress" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Áo sơ mi công sở lịch lãm", "https://via.placeholder.com/300x200?text=Shirt" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Giày thoải mái cho hoạt động thể thao", "https://via.placeholder.com/300x200?text=Sneakers" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Bàn làm việc gỗ hiện đại", "https://via.placeholder.com/300x200?text=Desk" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Ghế xoay thoải mái", "https://via.placeholder.com/300x200?text=Office+Chair" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Đèn LED tiết kiệm điện", "https://via.placeholder.com/300x200?text=Desk+Lamp" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Cây xanh trang trí bàn làm việc", "https://via.placeholder.com/300x200?text=Plant" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(8935), new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9074) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9195), new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9195) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9197), new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9197) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9198), new DateTime(2025, 6, 2, 11, 36, 29, 307, DateTimeKind.Utc).AddTicks(9199) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Máy tính xách tay mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Điện thoại thông minh mô tả chi tiết", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Chúa tể những chiếc nhẫn mô tả chi tiết", "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Xứ Cát mô tả chi tiết", "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Áo phông mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Quần jean mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Máy ảnh Kỹ thuật số mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Tai nghe Bluetooth mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Màn hình máy tính mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Bàn phím cơ mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Chuột không dây mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Tiểu thuyết trinh thám mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Sách nấu ăn mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Lịch sử Việt Nam mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Truyện tranh mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Váy đầm mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Áo sơ mi mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Giày thể thao mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Bàn làm việc mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Ghế văn phòng mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Đèn bàn mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Description", "ImgUrl" },
                values: new object[] { "Cây cảnh mini mô tả chi tiết", "https://images.unsplash.com/photo-1517336714731-489689fd1ca8" });
        }
    }
}
