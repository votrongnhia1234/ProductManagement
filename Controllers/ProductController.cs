using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Repositories;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IWebHostEnvironment webHostEnvironment,
            ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchTerm, int? categoryFilter, string sortOrder, int page = 1)
        {
            const int pageSize = 6;
            
            try
            {
                _logger.LogInformation("Loading products with search term: {SearchTerm}, category: {CategoryFilter}, sort: {SortOrder}, page: {Page}", 
                    searchTerm, categoryFilter, sortOrder, page);

                var products = await _productRepository.SearchAsync(searchTerm ?? "", categoryFilter, sortOrder ?? "name_asc");
                var categories = await _categoryRepository.GetAllAsync();

                _logger.LogInformation("Found {ProductCount} products and {CategoryCount} categories", 
                    products.Count, categories.Count);

                var totalProducts = products.Count;
                var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
                
                var pagedProducts = products
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Check if this is an AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Query.ContainsKey("ajax"))
                {
                    // Return JSON for AJAX requests
                    var jsonResult = new
                    {
                        products = pagedProducts.Select(p => new
                        {
                            id = p.Id,
                            productName = p.ProductName,
                            description = p.Description,
                            imgUrl = p.ImgUrl,
                            price = p.Price,
                            categoryId = p.CategoryId,
                            categoryName = p.Category?.Name
                        }),
                        pagination = new
                        {
                            currentPage = page,
                            totalPages = totalPages,
                            pageSize = pageSize,
                            totalCount = totalProducts
                        },
                        totalCount = totalProducts,
                        searchTerm = searchTerm,
                        categoryFilter = categoryFilter,
                        sortOrder = sortOrder
                    };

                    return Json(jsonResult);
                }

                // Return normal view for regular requests
                var viewModel = new ProductViewModel
                {
                    Products = pagedProducts,
                    Categories = categories,
                    SearchTerm = searchTerm ?? "",
                    CategoryFilter = categoryFilter,
                    SortOrder = sortOrder ?? "name_asc",
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách sản phẩm.";
                
                // Return empty view model in case of error
                var emptyViewModel = new ProductViewModel
                {
                    Products = new List<Product>(),
                    Categories = new List<Category>(),
                    SearchTerm = searchTerm ?? "",
                    CategoryFilter = categoryFilter,
                    SortOrder = sortOrder ?? "name_asc",
                    CurrentPage = page,
                    TotalPages = 0,
                    PageSize = pageSize
                };
                
                return View(emptyViewModel);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                _logger.LogInformation("Loading product details for ID: {ProductId}", id);
                
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    return NotFound();
                }
                
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID: {ProductId}", id);
                TempData["Error"] = "Có lỗi xảy ra khi tải thông tin sản phẩm.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = categories ?? new List<Category>();

                var model = new Product
                {
                    ProductName = "",
                    Description = "",
                    ImgUrl = "",
                    Price = 0,
                    CategoryId = 0
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create product page");
                TempData["Error"] = "Có lỗi xảy ra khi tải trang thêm sản phẩm.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Creating new product: {ProductName}", product.ProductName);

                    // Handle image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                        Directory.CreateDirectory(uploadsFolder);
                        
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                        
                        product.ImgUrl = "/images/products/" + uniqueFileName;
                    }
                    else if (string.IsNullOrEmpty(product.ImgUrl))
                    {
                        product.ImgUrl = "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=400";
                    }

                    await _productRepository.AddAsync(product);
                    
                    _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);
                    TempData["Success"] = "Sản phẩm đã được thêm thành công!";
                    return RedirectToAction(nameof(Index));
                }

                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = categories ?? new List<Category>();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product: {ProductName}", product.ProductName);
                TempData["Error"] = "Có lỗi xảy ra khi thêm sản phẩm.";
                
                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = categories ?? new List<Category>();
                return View(product);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit product page for ID: {ProductId}", id);
                TempData["Error"] = "Có lỗi xảy ra khi tải trang chỉnh sửa sản phẩm.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Updating product with ID: {ProductId}", product.Id);
                    
                    await _productRepository.UpdateAsync(product);
                    
                    _logger.LogInformation("Product updated successfully: {ProductId}", product.Id);
                    TempData["Success"] = "Sản phẩm đã được cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID: {ProductId}", product.Id);
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật sản phẩm.";
                
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                return View(product);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete product page for ID: {ProductId}", id);
                TempData["Error"] = "Có lỗi xảy ra khi tải trang xóa sản phẩm.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Deleting product with ID: {ProductId}", id);
                
                var result = await _productRepository.DeleteAsync(id);
                
                if (result)
                {
                    _logger.LogInformation("Product deleted successfully: {ProductId}", id);
                    TempData["Success"] = "Sản phẩm đã được xóa thành công!";
                }
                else
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
                    TempData["Error"] = "Không tìm thấy sản phẩm để xóa.";
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
                TempData["Error"] = "Có lỗi xảy ra khi xóa sản phẩm.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> QuickSearch(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                {
                    return Json(new List<object>());
                }

                var products = await _productRepository.SearchAsync(query, null, "name_asc");

                var results = products.Take(10).Select(p => new
                {
                    id = p.Id,
                    productName = p.ProductName,
                    imgUrl = p.ImgUrl,
                    price = p.Price,
                    categoryName = p.Category?.Name
                });

                return Json(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in quick search for query: {Query}", query);
                return Json(new List<object>());
            }
        }
    }
}
