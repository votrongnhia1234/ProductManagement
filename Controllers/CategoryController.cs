using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Repositories;

namespace ProductManagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchTerm, string sortOrder, int page = 1)
        {
            const int pageSize = 10;
            
            try
            {
                _logger.LogInformation("Loading categories with search term: {SearchTerm}, sort: {SortOrder}, page: {Page}", 
                    searchTerm, sortOrder, page);

                var allCategories = await _categoryRepository.GetAllAsync();
                
                // Load product count for each category
                foreach (var category in allCategories)
                {
                    var products = await _productRepository.GetByCategoryAsync(category.Id);
                    category.Products = products.ToList(); // Set the actual products list
                    
                    _logger.LogInformation("Category {CategoryName} (ID: {CategoryId}) has {ProductCount} products", 
                        category.Name, category.Id, products.Count());
                }
                
                // Search
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    allCategories = allCategories
                        .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Sort
                allCategories = sortOrder switch
                {
                    "name_desc" => allCategories.OrderByDescending(c => c.Name).ToList(),
                    "products_asc" => allCategories.OrderBy(c => c.Products?.Count ?? 0).ToList(),
                    "products_desc" => allCategories.OrderByDescending(c => c.Products?.Count ?? 0).ToList(),
                    _ => allCategories.OrderBy(c => c.Name).ToList()
                };

                var totalCategories = allCategories.Count;
                var totalPages = (int)Math.Ceiling(totalCategories / (double)pageSize);
                
                var pagedCategories = allCategories
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new CategoryViewModel
                {
                    Categories = pagedCategories,
                    SearchTerm = searchTerm ?? "",
                    SortOrder = sortOrder ?? "name_asc",
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalCategories
                };

                _logger.LogInformation("Loaded {CategoryCount} categories for display", pagedCategories.Count);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
                
                var emptyViewModel = new CategoryViewModel
                {
                    Categories = new List<Category>(),
                    SearchTerm = searchTerm ?? "",
                    SortOrder = sortOrder ?? "name_asc",
                    CurrentPage = page,
                    TotalPages = 0,
                    PageSize = pageSize,
                    TotalCount = 0
                };
                
                return View(emptyViewModel);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                // Get products in this category
                var products = await _productRepository.GetByCategoryAsync(id);
                category.Products = products.ToList();

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading category details for ID: {CategoryId}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            var model = new Category
            {
                Name = ""
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if category name already exists
                    var exists = await _categoryRepository.ExistsByNameAsync(category.Name);
                    if (exists)
                    {
                        ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
                        return View(category);
                    }

                    await _categoryRepository.AddAsync(category);
                    
                    _logger.LogInformation("Category created successfully: {CategoryName}", category.Name);
                    TempData["Success"] = "Danh mục đã được thêm thành công!";
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {CategoryName}", category.Name);
                return View(category);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit category page for ID: {CategoryId}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if category name already exists (excluding current category)
                    var exists = await _categoryRepository.ExistsByNameAsync(category.Name, category.Id);
                    if (exists)
                    {
                        ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
                        return View(category);
                    }

                    await _categoryRepository.UpdateAsync(category);
                    
                    _logger.LogInformation("Category updated successfully: {CategoryId}", category.Id);
                    TempData["Success"] = "Danh mục đã được cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID: {CategoryId}", category.Id);
                return View(category);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                // Get product count
                var products = await _productRepository.GetByCategoryAsync(id);
                category.Products = products.ToList();

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete category page for ID: {CategoryId}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete category with ID: {CategoryId}", id);
        
                // Check if category has products first
                var products = await _productRepository.GetByCategoryAsync(id);
                var productCount = products?.Count() ?? 0;
        
                _logger.LogInformation("Category {CategoryId} has {ProductCount} products", id, productCount);
        
                if (productCount > 0)
                {
                    _logger.LogWarning("Cannot delete category {CategoryId} because it has {ProductCount} products", id, productCount);
                    TempData["Success"] = $"Không thể xóa danh mục vì vẫn còn {productCount} sản phẩm trong danh mục này. Vui lòng chuyển hoặc xóa sản phẩm trước.";
                    return RedirectToAction(nameof(Index));
                }

                // Category has no products, safe to delete
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category with ID {CategoryId} not found", id);
                    TempData["Success"] = "Danh mục không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                // Force delete by removing from context directly
                var result = await _categoryRepository.DeleteAsync(id);
        
                if (result)
                {
                    _logger.LogInformation("Category {CategoryId} ({CategoryName}) deleted successfully", id, category.Name);
                    TempData["Success"] = $"Danh mục '{category.Name}' đã được xóa thành công!";
                }
                else
                {
                    _logger.LogError("Failed to delete category {CategoryId}", id);
                    TempData["Success"] = "Có lỗi xảy ra khi xóa danh mục.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID: {CategoryId}", id);
                TempData["Success"] = "Có lỗi xảy ra khi xóa danh mục.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
