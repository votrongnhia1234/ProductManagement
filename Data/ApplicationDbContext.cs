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
            var baseDate = new DateTime(2025, 6, 6);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptop & Máy tính", Description = "Laptop, PC, máy tính bảng và phụ kiện", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 2, Name = "Điện thoại & Tablet", Description = "Smartphone, tablet và phụ kiện di động", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 3, Name = "Âm thanh & Tai nghe", Description = "Tai nghe, loa, thiết bị âm thanh", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 4, Name = "Gaming & Esports", Description = "Thiết bị gaming, console, phụ kiện game", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 5, Name = "Camera & Quay phim", Description = "Máy ảnh, camera, thiết bị quay phim", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 6, Name = "Thiết bị mạng", Description = "Router, modem, thiết bị mạng và WiFi", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 7, Name = "Phụ kiện máy tính", Description = "Bàn phím, chuột, màn hình, webcam", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 8, Name = "Thiết bị thông minh", Description = "Smart home, IoT, thiết bị tự động hóa", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 9, Name = "Linh kiện máy tính", Description = "CPU, RAM, ổ cứng, card đồ họa", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 10, Name = "Phụ kiện di động", Description = "Ốp lưng, sạc, cáp, pin dự phòng", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 11, Name = "Thiết bị văn phòng", Description = "Máy in, scanner, máy chiếu, thiết bị VP", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Category { Id = 12, Name = "Đồng hồ thông minh", Description = "Smartwatch, fitness tracker, wearable", CreatedAt = baseDate, UpdatedAt = baseDate }
            );
        }

        private void SeedProducts(ModelBuilder builder)
        {
            var baseDate = new DateTime(2025, 6, 6);

            builder.Entity<Product>().HasData(
                // Laptop & Máy tính (Category 1)
                new Product { Id = 1, ProductName = "MacBook Pro M3 14 inch", Description = "Laptop Apple M3 chip, 16GB RAM, 512GB SSD", Price = 4500.00M, CategoryId = 1, ImgUrl = "/images/products/macbook-pro-m3.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 2, ProductName = "Dell XPS 13 Plus", Description = "Laptop Dell Intel Core i7, 16GB RAM, 1TB SSD", Price = 3500.00M, CategoryId = 1, ImgUrl = "/images/products/dell-xps-13.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 3, ProductName = "ASUS ROG Strix G15", Description = "Gaming laptop AMD Ryzen 7, RTX 4060, 16GB RAM", Price = 2800.00M, CategoryId = 1, ImgUrl = "/images/products/asus-rog-strix.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 4, ProductName = "iPad Pro 12.9 inch M2", Description = "Máy tính bảng Apple M2, 256GB, WiFi + Cellular", Price = 32.00M, CategoryId = 1, ImgUrl = "/images/products/ipad-pro-m2.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Điện thoại & Tablet (Category 2)
                new Product { Id = 5, ProductName = "iPhone 15 Pro Max", Description = "iPhone 15 Pro Max 256GB, Titanium Natural", Price = 3400.00M, CategoryId = 2, ImgUrl = "/images/products/iphone-15-pro-max.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 6, ProductName = "Samsung Galaxy S24 Ultra", Description = "Galaxy S24 Ultra 512GB, S Pen, AI Camera", Price = 3100.00M, CategoryId = 2, ImgUrl = "/images/products/galaxy-s24-ultra.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 7, ProductName = "Google Pixel 8 Pro", Description = "Pixel 8 Pro 256GB, AI Photography, Pure Android", Price = 2500.00M, CategoryId = 2, ImgUrl = "/images/products/pixel-8-pro.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 8, ProductName = "Xiaomi 14 Ultra", Description = "Xiaomi 14 Ultra 512GB, Leica Camera, Snapdragon 8 Gen 3", Price = 2200.00M, CategoryId = 2, ImgUrl = "/images/products/xiaomi-14-ultra.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Âm thanh & Tai nghe (Category 3)
                new Product { Id = 9, ProductName = "AirPods Pro 2nd Gen", Description = "Tai nghe Apple AirPods Pro, Active Noise Cancelling", Price = 6500.00M, CategoryId = 3, ImgUrl = "/images/products/airpods-pro-2.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 10, ProductName = "Sony WH-1000XM5", Description = "Tai nghe over-ear Sony, chống ồn hàng đầu", Price = 8500.00M, CategoryId = 3, ImgUrl = "/images/products/sony-wh1000xm5.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 11, ProductName = "JBL Charge 5", Description = "Loa Bluetooth JBL chống nước, bass mạnh", Price = 3500.00M, CategoryId = 3, ImgUrl = "/images/products/jbl-charge-5.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 12, ProductName = "Bose QuietComfort 45", Description = "Tai nghe Bose chống ồn, âm thanh Hi-Fi", Price = 7500.00M, CategoryId = 3, ImgUrl = "/images/products/bose-qc45.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Gaming & Esports (Category 4)
                new Product { Id = 13, ProductName = "PlayStation 5 Slim", Description = "Console PS5 Slim 1TB, 4K Gaming, Ray Tracing", Price = 14000.00M, CategoryId = 4, ImgUrl = "/images/products/ps5-slim.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 14, ProductName = "Xbox Series X", Description = "Console Xbox Series X 1TB, 4K 120fps Gaming", Price = 13500.00M, CategoryId = 4, ImgUrl = "/images/products/xbox-series-x.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 15, ProductName = "Razer DeathAdder V3 Pro", Description = "Chuột gaming Razer wireless, sensor Focus Pro 30K", Price = 3500.00M, CategoryId = 4, ImgUrl = "/images/products/razer-deathadder-v3.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 16, ProductName = "SteelSeries Apex Pro TKL", Description = "Bàn phím cơ gaming, OmniPoint switches", Price = 4500.00M, CategoryId = 4, ImgUrl = "/images/products/steelseries-apex-pro.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Camera & Quay phim (Category 5)
                new Product { Id = 17, ProductName = "Canon EOS R6 Mark II", Description = "Máy ảnh mirrorless Canon, 24.2MP, 4K Video", Price = 4200.00M, CategoryId = 5, ImgUrl = "/images/products/canon-r6-mark2.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 18, ProductName = "Sony A7 IV", Description = "Máy ảnh Sony Alpha 33MP, 4K 60p, Real-time Eye AF", Price = 4800.00M, CategoryId = 5, ImgUrl = "/images/products/sony-a7-iv.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 19, ProductName = "DJI Mini 4 Pro", Description = "Drone DJI 4K HDR, 3-axis gimbal, 45 phút bay", Price = 1800.00M, CategoryId = 5, ImgUrl = "/images/products/dji-mini-4-pro.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 20, ProductName = "GoPro Hero 12 Black", Description = "Action camera GoPro 5.3K, HyperSmooth 6.0", Price = 1200.00M, CategoryId = 5, ImgUrl = "/images/products/gopro-hero-12.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Thiết bị mạng (Category 6)
                new Product { Id = 21, ProductName = "ASUS AX6000 WiFi 6E", Description = "Router ASUS WiFi 6E, 6000Mbps, Mesh ready", Price = 8000.00M, CategoryId = 6, ImgUrl = "/images/products/asus-ax6000.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 22, ProductName = "TP-Link Archer AX73", Description = "Router TP-Link WiFi 6, 5400Mbps, OneMesh", Price = 3000.00M, CategoryId = 6, ImgUrl = "/images/products/tplink-ax73.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 23, ProductName = "Netgear Orbi AX6000", Description = "Hệ thống Mesh WiFi 6, phủ sóng 500m²", Price = 1200.00M, CategoryId = 6, ImgUrl = "/images/products/netgear-orbi.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Phụ kiện máy tính (Category 7)
                new Product { Id = 24, ProductName = "LG UltraWide 34WP65C", Description = "Màn hình cong 34 inch, 3440x1440, USB-C", Price = 8000.00M, CategoryId = 7, ImgUrl = "/images/products/lg-ultrawide-34.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 25, ProductName = "Logitech MX Master 3S", Description = "Chuột wireless Logitech, 8000 DPI, USB-C", Price = 2000.00M, CategoryId = 7, ImgUrl = "/images/products/logitech-mx-master-3s.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 26, ProductName = "Keychron K8 Pro", Description = "Bàn phím cơ wireless, hot-swap, RGB", Price = 3000.00M, CategoryId = 7, ImgUrl = "/images/products/keychron-k8-pro.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 27, ProductName = "Logitech C920s Pro HD", Description = "Webcam Logitech 1080p, auto-focus, stereo audio", Price = 1000.00M, CategoryId = 7, ImgUrl = "/images/products/logitech-c920s.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Thiết bị thông minh (Category 8)
                new Product { Id = 28, ProductName = "Amazon Echo Dot 5th Gen", Description = "Loa thông minh Amazon Alexa, điều khiển giọng nói", Price = 100.00M, CategoryId = 8, ImgUrl = "/images/products/echo-dot-5.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 29, ProductName = "Google Nest Hub Max", Description = "Màn hình thông minh Google 10 inch, camera", Price = 6500.00M, CategoryId = 8, ImgUrl = "/images/products/nest-hub-max.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 30, ProductName = "Philips Hue White Ambiance", Description = "Bộ đèn LED thông minh Philips, điều khiển màu sắc", Price = 2800.00M, CategoryId = 8, ImgUrl = "/images/products/philips-hue.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Linh kiện máy tính (Category 9)
                new Product { Id = 31, ProductName = "AMD Ryzen 9 7900X", Description = "CPU AMD 12 cores, 24 threads, 5.6GHz boost", Price = 1200.00M, CategoryId = 9, ImgUrl = "/images/products/amd-ryzen-9-7900x.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 32, ProductName = "NVIDIA RTX 4070 Super", Description = "Card đồ họa NVIDIA 12GB GDDR6X, DLSS 3", Price = 1800.00M, CategoryId = 9, ImgUrl = "/images/products/rtx-4070-super.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 33, ProductName = "Corsair Vengeance DDR5", Description = "RAM Corsair 32GB (2x16GB) DDR5-5600, RGB", Price = 4000.00M, CategoryId = 9, ImgUrl = "/images/products/corsair-vengeance-ddr5.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 34, ProductName = "Samsung 980 PRO 2TB", Description = "SSD NVMe Samsung 2TB, 7000MB/s read speed", Price = 5000.00M, CategoryId = 9, ImgUrl = "/images/products/samsung-980-pro.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Phụ kiện di động (Category 10)
                new Product { Id = 35, ProductName = "Anker PowerCore 26800", Description = "Pin sạc dự phòng Anker 26800mAh, sạc nhanh PD", Price = 1000.00M, CategoryId = 10, ImgUrl = "/images/products/anker-powercore.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 36, ProductName = "Belkin MagSafe 3-in-1", Description = "Đế sạc không dây Belkin cho iPhone, AirPods, Apple Watch", Price = 3000.00M, CategoryId = 10, ImgUrl = "/images/products/belkin-magsafe-3in1.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 37, ProductName = "Peak Design Mobile Tripod", Description = "Chân máy Peak Design cho smartphone, gấp gọn", Price = 1000.00M, CategoryId = 10, ImgUrl = "/images/products/peak-design-tripod.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Thiết bị văn phòng (Category 11)
                new Product { Id = 38, ProductName = "HP LaserJet Pro M404n", Description = "Máy in laser HP đen trắng, tốc độ 38 trang/phút", Price = 4500.00M, CategoryId = 11, ImgUrl = "/images/products/hp-laserjet-m404n.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 39, ProductName = "Epson EcoTank L3250", Description = "Máy in phun màu Epson, in scan copy, WiFi", Price = 3000.00M, CategoryId = 11, ImgUrl = "/images/products/epson-l3250.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 40, ProductName = "BenQ MW560 Projector", Description = "Máy chiếu BenQ WXGA 4000 lumens, HDMI", Price = 1200.00M, CategoryId = 11, ImgUrl = "/images/products/benq-mw560.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },

                // Đồng hồ thông minh (Category 12)
                new Product { Id = 41, ProductName = "Apple Watch Series 9", Description = "Apple Watch 45mm, GPS + Cellular, Always-On display", Price = 1200.00M, CategoryId = 12, ImgUrl = "/images/products/apple-watch-series-9.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 42, ProductName = "Samsung Galaxy Watch 6", Description = "Galaxy Watch 6 44mm, Wear OS, health tracking", Price = 8500.00M, CategoryId = 12, ImgUrl = "/images/products/galaxy-watch-6.jpg", CreatedAt = baseDate, UpdatedAt = baseDate },
                new Product { Id = 43, ProductName = "Garmin Fenix 7X Solar", Description = "Đồng hồ thể thao Garmin, GPS, pin năng lượng mặt trời", Price = 1800.00M, CategoryId = 12, ImgUrl = "/images/products/garmin-fenix-7x.jpg", CreatedAt = baseDate, UpdatedAt = baseDate }
            );
        }
    }
}