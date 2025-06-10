using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

namespace ProductManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Product
            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Category
            builder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Order
            builder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderItem
            builder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Product)
                      .WithMany(p => p.OrderItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed default roles
            SeedRoles(builder);
            SeedCategories(builder);
            SeedProducts(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole { Id = "3", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }

        private void SeedCategories(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Điện tử", Description = "Thiết bị và tiện ích điện tử", CreatedAt = new DateTime(2025, 6, 2), UpdatedAt = new DateTime(2025, 6, 2) },
                new Category { Id = 2, Name = "Sách", Description = "Nhiều loại sách và tiểu thuyết", CreatedAt = new DateTime(2025, 6, 2), UpdatedAt = new DateTime(2025, 6, 2) },
                new Category { Id = 3, Name = "Thời trang", Description = "Quần áo và các mặt hàng thời trang", CreatedAt = new DateTime(2025, 6, 2), UpdatedAt = new DateTime(2025, 6, 2) },
                new Category { Id = 4, Name = "Nhà cửa & Vườn", Description = "Đồ dùng cho nhà cửa và sân vườn", CreatedAt = new DateTime(2025, 6, 2), UpdatedAt = new DateTime(2025, 6, 2) }
            );
        }

        private void SeedProducts(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product {
                    Id = 1,
                    ProductName = "Máy tính xách tay",
                    Description = "Máy tính xách tay hiệu năng cao",
                    Price = 1200.00M,
                    CategoryId = 1,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 2,
                    ProductName = "Điện thoại thông minh",
                    Description = "Điện thoại thông minh mẫu mới nhất",
                    Price = 800.00M,
                    CategoryId = 1,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 3,
                    ProductName = "Chúa tể những chiếc nhẫn",
                    Description = "Tiểu thuyết giả tưởng sử thi",
                    Price = 20.00M,
                    CategoryId = 2,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 4,
                    ProductName = "Xứ Cát",
                    Description = "Khoa học viễn tưởng kinh điển",
                    Price = 15.00M,
                    CategoryId = 2,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 5,
                    ProductName = "Áo phông",
                    Description = "Áo phông cotton thoải mái",
                    Price = 25.00M,
                    CategoryId = 3,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 6,
                    ProductName = "Quần jean",
                    Description = "Quần jean vải bền",
                    Price = 50.00M,
                    CategoryId = 3,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 7,
                    ProductName = "Máy ảnh Kỹ thuật số",
                    Description = "Máy ảnh độ phân giải cao",
                    Price = 650.00M,
                    CategoryId = 1,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 8,
                    ProductName = "Tai nghe Bluetooth",
                    Description = "Tai nghe không dây chống ồn",
                    Price = 150.00M,
                    CategoryId = 1,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 9,
                    ProductName = "Màn hình máy tính",
                    Description = "Màn hình Full HD 27 inch",
                    Price = 300.00M,
                    CategoryId = 1,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 10,
                    ProductName = "Bàn phím cơ",
                    Description = "Bàn phím cơ với đèn nền RGB",
                    Price = 100.00M,
                    CategoryId = 1,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 11,
                    ProductName = "Chuột không dây",
                    Description = "Chuột quang không dây",
                    Price = 25.00M,
                    CategoryId = 1,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 12,
                    ProductName = "Tiểu thuyết trinh thám",
                    Description = "Câu chuyện hấp dẫn đầy bí ẩn",
                    Price = 18.00M,
                    CategoryId = 2,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 13,
                    ProductName = "Sách nấu ăn",
                    Description = "Tuyển tập công thức món ngon",
                    Price = 30.00M,
                    CategoryId = 2,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 14,
                    ProductName = "Lịch sử Việt Nam",
                    Description = "Tìm hiểu về lịch sử hào hùng",
                    Price = 25.00M,
                    CategoryId = 2,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 15,
                    ProductName = "Truyện tranh",
                    Description = "Bộ sưu tập truyện tranh vui nhộn",
                    Price = 10.00M,
                    CategoryId = 2,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 16,
                    ProductName = "Váy đầm",
                    Description = "Váy thời trang cho nữ",
                    Price = 40.00M,
                    CategoryId = 3,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 17,
                    ProductName = "Áo sơ mi",
                    Description = "Áo sơ mi công sở lịch lãm",
                    Price = 35.00M,
                    CategoryId = 3,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 18,
                    ProductName = "Giày thể thao",
                    Description = "Giày thoải mái cho hoạt động thể thao",
                    Price = 70.00M,
                    CategoryId = 3,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 19,
                    ProductName = "Bàn làm việc",
                    Description = "Bàn làm việc gỗ hiện đại",
                    Price = 150.00M,
                    CategoryId = 4,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 20,
                    ProductName = "Ghế văn phòng",
                    Description = "Ghế xoay thoải mái",
                    Price = 80.00M,
                    CategoryId = 4,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 21,
                    ProductName = "Đèn bàn",
                    Description = "Đèn LED tiết kiệm điện",
                    Price = 30.00M,
                    CategoryId = 4,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                },
                new Product {
                    Id = 22,
                    ProductName = "Cây cảnh mini",
                    Description = "Cây xanh trang trí bàn làm việc",
                    Price = 10.00M,
                    CategoryId = 4,
                    ImgUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",
                    CreatedAt = new DateTime(2025, 6, 2),
                    UpdatedAt = new DateTime(2025, 6, 2)
                }
            );
        }
    }
}
