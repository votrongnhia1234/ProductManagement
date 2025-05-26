using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

namespace ProductManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
                
                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(500);
                
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                // Configure relationship
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Add index for better performance
                entity.HasIndex(p => p.ProductName);
                entity.HasIndex(p => p.CategoryId);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                // Add index
                entity.HasIndex(c => c.Name).IsUnique();
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            var categories = new[]
            {
                new Category { Id = 1, Name = "Nội thất phòng khách" },
                new Category { Id = 2, Name = "Nội thất phòng ngủ" },
                new Category { Id = 3, Name = "Nội thất nhà bếp" },
                new Category { Id = 4, Name = "Đồ trang trí" },
                new Category { Id = 5, Name = "Thiết bị thông minh" }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seed Products
            var products = new[]
            {
                new Product 
                { 
                    Id = 1, 
                    ProductName = "Sofa hiện đại", 
                    Description = "Sofa 3 chỗ ngồi thiết kế hiện đại, chất liệu da cao cấp", 
                    ImgUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400", 
                    Price = 15000000, 
                    CategoryId = 1 
                },
                new Product 
                { 
                    Id = 2, 
                    ProductName = "Bàn cà phê gỗ", 
                    Description = "Bàn cà phê gỗ tự nhiên cao cấp, thiết kế tối giản", 
                    ImgUrl = "https://images.unsplash.com/photo-1549497538-303791108f95?w=400", 
                    Price = 3500000, 
                    CategoryId = 1 
                },
                new Product 
                { 
                    Id = 3, 
                    ProductName = "Giường ngủ king size", 
                    Description = "Giường ngủ king size với đầu giường bọc da, khung gỗ sồi", 
                    ImgUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?w=400", 
                    Price = 12000000, 
                    CategoryId = 2 
                },
                new Product 
                { 
                    Id = 4, 
                    ProductName = "Tủ quần áo", 
                    Description = "Tủ quần áo 4 cánh gỗ công nghiệp, có gương và ngăn kéo", 
                    ImgUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=400", 
                    Price = 8500000, 
                    CategoryId = 2 
                },
                new Product 
                { 
                    Id = 5, 
                    ProductName = "Bộ bàn ăn", 
                    Description = "Bộ bàn ăn 6 ghế gỗ sồi, thiết kế sang trọng", 
                    ImgUrl = "https://images.unsplash.com/photo-1449247709967-d4461a6a6103?w=400", 
                    Price = 9500000, 
                    CategoryId = 3 
                },
                new Product 
                { 
                    Id = 6, 
                    ProductName = "Kệ tivi hiện đại", 
                    Description = "Kệ tivi gỗ công nghiệp, có ngăn chứa đồ", 
                    ImgUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400", 
                    Price = 4200000, 
                    CategoryId = 1 
                },
                new Product 
                { 
                    Id = 7, 
                    ProductName = "Tủ bếp cao cấp", 
                    Description = "Tủ bếp gỗ công nghiệp chống ẩm, mặt đá granite", 
                    ImgUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400", 
                    Price = 25000000, 
                    CategoryId = 3 
                },
                new Product 
                { 
                    Id = 8, 
                    ProductName = "Đèn trang trí", 
                    Description = "Đèn chùm pha lê cao cấp, ánh sáng LED", 
                    ImgUrl = "https://images.unsplash.com/photo-1524484485831-a92ffc0de03f?w=400", 
                    Price = 1800000, 
                    CategoryId = 4 
                }
            };

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
