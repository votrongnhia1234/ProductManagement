using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private static List<Category> _categories = new();
        private static int _nextCategoryId = 1;
        private static bool _initialized = false;

        public CategoryRepository()
        {
            if (!_initialized)
            {
                InitializeData();
                _initialized = true;
            }
        }

        private static void InitializeData()
        {
            _categories = new List<Category>
            {
                new Category { Id = _nextCategoryId++, Name = "Nội thất phòng khách" },
                new Category { Id = _nextCategoryId++, Name = "Nội thất phòng ngủ" },
                new Category { Id = _nextCategoryId++, Name = "Nội thất nhà bếp" },
                new Category { Id = _nextCategoryId++, Name = "Đồ trang trí" },
                new Category { Id = _nextCategoryId++, Name = "Thiết bị thông minh" }
            };
        }

        public Task<List<Category>> GetAllAsync()
        {
            return Task.FromResult(_categories?.ToList() ?? new List<Category>());
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            var category = _categories?.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(category);
        }

        public Task<Category> AddAsync(Category category)
        {
            if (_categories == null)
            {
                InitializeData();
            }
            
            category.Id = _nextCategoryId++;
            _categories.Add(category);
            return Task.FromResult(category);
        }

        public Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = _categories?.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                return Task.FromResult(existingCategory);
            }
            throw new ArgumentException($"Category with ID {category.Id} not found");
        }

        public Task<bool> DeleteAsync(int id)
        {
            var category = _categories?.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _categories.Remove(category);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(_categories?.Any(c => c.Id == id) ?? false);
        }

        public Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            if (_categories == null) return Task.FromResult(false);
            
            var query = _categories.Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }
            
            return Task.FromResult(query.Any());
        }

        public Task<int> GetProductCountAsync(int categoryId)
        {
            // This would need access to products, for now return 0
            // In a real scenario, you'd inject IProductRepository or use a service
            return Task.FromResult(0);
        }
    }
}
