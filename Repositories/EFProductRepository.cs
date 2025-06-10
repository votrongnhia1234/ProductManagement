using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync(string? searchTerm = null, int? categoryId = null, int page = 1, int pageSize = 20)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm) ||
                                        p.Description!.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetProductCountAsync(string? searchTerm = null, int? categoryId = null)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm) ||
                                        p.Description!.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> GetProductCountByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .CountAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId, int count = 10)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Product>> GetFeaturedProductsAsync(int count = 8)
        {
            // Lấy sản phẩm mới nhất làm sản phẩm nổi bật
            return await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm, int maxResults = 10)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return new List<Product>();

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductName.Contains(searchTerm) ||
                           p.Description!.Contains(searchTerm))
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<decimal> GetAveragePriceAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Any() ? products.Average(p => p.Price) : 0;
        }

        public async Task<Product?> GetMostExpensiveProductAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Price)
                .FirstOrDefaultAsync();
        }

        public async Task<Product?> GetCheapestProductAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Price)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsInPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .OrderBy(p => p.Price)
                .ToListAsync();
        }

        public async Task<int> GetTotalProductsCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<Product>> GetRecentProductsAsync(int count = 5)
        {
            return await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> UpdateProductStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            // Nếu có trường Stock trong tương lai
            // product.Stock = quantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> GetTopSellingProductsAsync(int count = 10)
        {
            // Lấy sản phẩm bán chạy dựa trên số lượng trong OrderItems
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.OrderItems)
                .OrderByDescending(p => p.OrderItems.Sum(oi => oi.Quantity))
                .Take(count)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetProductCountByCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .GroupBy(p => p.Category!.Name)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
}
