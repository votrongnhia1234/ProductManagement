using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagement.Models;
using ProductManagement.Repositories;
using ProductManagement.Areas.Admin.Models;

namespace ProductManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string searchTerm, int? categoryFilter, int page = 1)
        {
            const int pageSize = 12;

            var products = await _productRepository.GetProductsAsync(searchTerm, categoryFilter, page, pageSize);
            var totalProducts = await _productRepository.GetProductCountAsync(searchTerm, categoryFilter);
            var categories = await _categoryRepository.GetCategoriesAsync();

            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var viewModel = new ProductManagementViewModel
            {
                Products = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImgUrl = p.ImgUrl,
                    CategoryName = p.Category?.Name ?? "Không có",
                    CategoryId = p.CategoryId,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList(),
                Categories = categories.ToList(),
                SearchTerm = searchTerm ?? string.Empty,
                CategoryFilter = categoryFilter,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalProducts = totalProducts
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailsViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                ImgUrl = product.ImgUrl,
                CategoryName = product.Category?.Name ?? "Không có",
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            var viewModel = new CreateProductViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName = model.ProductName,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Handle image upload
                if (model.ImageFile != null)
                {
                    product.ImgUrl = await SaveImageAsync(model.ImageFile);
                }

                await _productRepository.CreateProductAsync(product);
                TempData["Success"] = "Sản phẩm đã được tạo thành công.";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetCategoriesAsync();
            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetCategoriesAsync();
            var viewModel = new EditProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CurrentImageUrl = product.ImgUrl,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == product.CategoryId
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await _productRepository.GetProductByIdAsync(model.Id);
                if (product == null)
                {
                    return NotFound();
                }

                product.ProductName = model.ProductName;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.UpdatedAt = DateTime.UtcNow;

                // Handle image upload
                if (model.ImageFile != null)
                {
                    // Delete old image
                    if (!string.IsNullOrEmpty(product.ImgUrl))
                    {
                        DeleteImage(product.ImgUrl);
                    }

                    product.ImgUrl = await SaveImageAsync(model.ImageFile);
                }

                await _productRepository.UpdateProductAsync(product);
                TempData["Success"] = "Sản phẩm đã được cập nhật thành công.";
                return RedirectToAction(nameof(Details), new { id = product.Id });
            }

            var categories = await _categoryRepository.GetCategoriesAsync();
            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.CategoryId
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Check if product has orders
            // This would require checking OrderItems
            // For now, we'll allow deletion

            // Delete image file
            if (!string.IsNullOrEmpty(product.ImgUrl))
            {
                DeleteImage(product.ImgUrl);
            }

            await _productRepository.DeleteProductAsync(id);
            TempData["Success"] = "Sản phẩm đã được xóa thành công.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/products/" + uniqueFileName;
        }

        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
