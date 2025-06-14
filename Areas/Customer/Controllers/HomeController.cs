using Microsoft.AspNetCore.Mvc;
using ProductManagement.Repositories;
using ProductManagement.Areas.Customer.Models;

namespace ProductManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(string searchTerm, int? categoryFilter, int page = 1)
        {
            const int pageSize = 12;

            var products = await _productRepository.GetProductsAsync(searchTerm, categoryFilter, page, pageSize);
            var totalProducts = await _productRepository.GetProductCountAsync(searchTerm, categoryFilter);
            var categories = await _categoryRepository.GetCategoriesAsync();
            var featuredProducts = await _productRepository.GetFeaturedProductsAsync(8);

            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var viewModel = new CustomerHomeViewModel
            {
                Products = products.Select(p => new CustomerProductViewModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImgUrl = p.ImgUrl,
                    CategoryName = p.Category?.Name ?? "Không có"
                }).ToList(),
                FeaturedProducts = featuredProducts.Select(p => new CustomerProductViewModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImgUrl = p.ImgUrl,
                    CategoryName = p.Category?.Name ?? "Không có"
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

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Get related products from same category
            var relatedProducts = await _productRepository.GetProductsByCategoryAsync(product.CategoryId, 4);
            relatedProducts = relatedProducts.Where(p => p.Id != id).Take(4).ToList();

            // Pass the product directly to the view as the view expects a Product model
            return View(product);
        }

        [HttpGet]
        public IActionResult Search(string q)
        {
            // Chuyển hướng về Index với searchTerm
            return RedirectToAction("Index", new { searchTerm = q });
        }
    }
}
