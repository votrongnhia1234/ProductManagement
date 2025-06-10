using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Repositories;
using ProductManagement.Areas.Admin.Models;

namespace ProductManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        private const string CategoryNotFoundMessage = "Không tìm thấy danh mục.";
        private const string CannotDeleteCategoryWithProductsMessage = "Không thể xóa danh mục có {0} sản phẩm. Vui lòng di chuyển hoặc xóa sản phẩm trước.";
        private const string CategoryDeletedSuccessMessage = "Danh mục đã được xóa thành công.";

        public CategoryController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            const int pageSize = 10;

            var categories = await _categoryRepository.GetCategoriesAsync(searchTerm, page, pageSize);
            var totalCategories = await _categoryRepository.GetCategoryCountAsync(searchTerm);
            var totalPages = (int)Math.Ceiling(totalCategories / (double)pageSize);

            var categoryViewModels = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                var productCount = await _productRepository.GetProductCountByCategoryAsync(category.Id);
                categoryViewModels.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedAt = category.CreatedAt,
                    ProductCount = productCount
                });
            }

            var viewModel = new CategoryManagementViewModel
            {
                Categories = categoryViewModels,
                SearchTerm = searchTerm ?? string.Empty,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCategories = totalCategories
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var products = await _productRepository.GetProductsByCategoryAsync(id);
            var viewModel = new CategoryDetailsViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                Products = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ImgUrl = p.ImgUrl,
                    CreatedAt = p.CreatedAt
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if category name already exists
                var existingCategory = await _categoryRepository.GetCategoryByNameAsync(model.Name);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
                    return View(model);
                }

                var category = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreatedAt = DateTime.UtcNow
                };

                await _categoryRepository.CreateCategoryAsync(category);
                TempData["Success"] = "Danh mục đã được tạo thành công.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(model.Id);
                if (category == null)
                {
                    return NotFound();
                }

                // Check if new name conflicts with existing category
                var existingCategory = await _categoryRepository.GetCategoryByNameAsync(model.Name);
                if (existingCategory != null && existingCategory.Id != model.Id)
                {
                    ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
                    return View(model);
                }

                category.Name = model.Name;
                category.Description = model.Description;

                await _categoryRepository.UpdateCategoryAsync(category);
                TempData["Success"] = "Danh mục đã được cập nhật thành công.";
                return RedirectToAction(nameof(Details), new { id = category.Id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return Json(new { success = false, message = CategoryNotFoundMessage });
            }

            // Check if category has products
            var productCount = await _productRepository.GetProductCountByCategoryAsync(id);
            if (productCount > 0)
            {
                return Json(new { success = false, message = string.Format(CannotDeleteCategoryWithProductsMessage, productCount) });
            }

            await _categoryRepository.DeleteCategoryAsync(id);
            return Json(new { success = true, message = CategoryDeletedSuccessMessage });
        }
    }
}
