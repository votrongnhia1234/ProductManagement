using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync(string? searchTerm = null, int page = 1, int pageSize = 20);
        Task<List<Category>> GetCategoriesAsync(); // For dropdown/navigation
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryByNameAsync(string name);
        Task<int> GetCategoryCountAsync(string? searchTerm = null);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
