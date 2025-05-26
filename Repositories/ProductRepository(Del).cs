using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = new();
        private static List<Category> _categories = new();
        private static int _nextProductId = 1;
        private static int _nextCategoryId = 1;

        static ProductRepository()
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

            // Initialize products
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

        public Task<List<Product>> GetAllAsync()
        {
            return Task.FromResult(_products.ToList());
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<Product> AddAsync(Product product)
        {
            product.Id = _nextProductId++;
            product.Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Description = product.Description;
                existingProduct.ImgUrl = product.ImgUrl;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
                return Task.FromResult(existingProduct);
            }
            throw new ArgumentException($"Product with ID {product.Id} not found");
        }

        public Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<Product>> SearchAsync(string searchTerm, int? categoryId, string sortOrder)
        {
            var query = _products.AsQueryable();

            // Search by product name
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by category
            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            // Sort
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(p => p.ProductName),
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.ProductName)
            };

            return Task.FromResult(query.ToList());
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(_products.Any(p => p.Id == id));
        }

        public Task<int> GetCountAsync()
        {
            return Task.FromResult(_products.Count);
        }

        public Task<List<Product>> GetByCategoryAsync(int categoryId)
        {
            var products = _products.Where(p => p.CategoryId == categoryId).ToList();
            return Task.FromResult(products);
        }

        public Task<List<Product>> GetTopProductsAsync(int count)
        {
            var products = _products.OrderByDescending(p => p.Price).Take(count).ToList();
            return Task.FromResult(products);
        }

        public List<Category> GetAllCategories()
        {
            return _categories.ToList();
        }
    }
}
