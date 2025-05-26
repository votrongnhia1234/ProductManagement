using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<List<Product>> SearchAsync(string searchTerm, int? categoryId, string sortOrder);
        Task<bool> ExistsAsync(int id);
        Task<int> GetCountAsync();
        Task<List<Product>> GetByCategoryAsync(int categoryId);
        Task<List<Product>> GetTopProductsAsync(int count);
    }
}
