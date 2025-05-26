using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    // Đổi tên để tránh conflict với interface
    public class InMemoryProductRepository
    {
        private static List<Product> _products = new();
        private static List<Category> _categories = new();
        private static int _nextProductId = 1;
        private static int _nextCategoryId = 1;

        static InMemoryProductRepository()
        {
            InitializeData();
        }

        private static void InitializeData()
        {
            // Initialize categories
            _categories = new List<Category>
            {
                new Category { Id = _nextCategoryId++, Name = "Nội thất phòng khách" },
                new Category { Id = _nextCategoryId++, Name = "Nội thất phòng ngủ" },
                new Category { Id = _nextCategoryId++, Name = "Nội thất nhà bếp" },
                new Category { Id = _nextCategoryId++, Name = "Đồ trang trí" },
                new Category { Id = _nextCategoryId++, Name = "Thiết bị thông minh" }
            };

            // Initialize products - CHỈ 5 SẢN PHẨM (đây là lý do chỉ hiển thị 5)
            _products = new List<Product>
            {
                new Product { Id = _nextProductId++, ProductName = "Sofa hiện đại", Description = "Sofa 3 chỗ ngồi thiết kế hiện đại", ImgUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400", Price = 15000000, CategoryId = 1 },
                new Product { Id = _nextProductId++, ProductName = "Bàn cà phê gỗ", Description = "Bàn cà phê gỗ tự nhiên cao cấp", ImgUrl = "https://images.unsplash.com/photo-1549497538-303791108f95?w=400", Price = 3500000, CategoryId = 1 },
                new Product { Id = _nextProductId++, ProductName = "Giường ngủ king size", Description = "Giường ngủ king size với đầu giường bọc da", ImgUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?w=400", Price = 12000000, CategoryId = 2 },
                new Product { Id = _nextProductId++, ProductName = "Tủ quần áo", Description = "Tủ quần áo 4 cánh gỗ công nghiệp", ImgUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=400", Price = 8500000, CategoryId = 2 },
                new Product { Id = _nextProductId++, ProductName = "Bộ bàn ăn", Description = "Bộ bàn ăn 6 ghế gỗ sồi", ImgUrl = "https://images.unsplash.com/photo-1449247709967-d4461a6a6103?w=400", Price = 9500000, CategoryId = 3 }
            };

            // Set category references
            foreach (var product in _products)
            {
                product.Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
            }
        }

        // Các method khác...
    }
}
