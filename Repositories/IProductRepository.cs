using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(string? searchTerm = null, int? categoryId = null, int page = 1, int pageSize = 20);
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> GetProductCountAsync(string? searchTerm = null, int? categoryId = null);
        Task<int> GetProductCountByCategoryAsync(int categoryId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId, int count = 10);
        Task<List<Product>> GetFeaturedProductsAsync(int count = 8);

        // Thêm các method mới
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        Task<bool> ProductExistsAsync(int id);
        Task<List<Product>> SearchProductsAsync(string searchTerm, int maxResults = 10);
        Task<decimal> GetAveragePriceAsync();
        Task<Product?> GetMostExpensiveProductAsync();
        Task<Product?> GetCheapestProductAsync();
        Task<List<Product>> GetProductsInPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<int> GetTotalProductsCountAsync();
        Task<List<Product>> GetRecentProductsAsync(int count = 5);
        Task<bool> UpdateProductStockAsync(int productId, int quantity);
        Task<List<Product>> GetTopSellingProductsAsync(int count = 10);
        Task<Dictionary<string, int>> GetProductCountByCategoryAsync();
    }
}
