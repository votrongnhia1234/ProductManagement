using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Repositories;
using ProductManagement.Areas.Admin.Models;
using ProductManagement.Models;

namespace ProductManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")]
    public class DashboardController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;

        public DashboardController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var totalProducts = await _productRepository.GetTotalProductsCountAsync();
            var totalCategories = await _categoryRepository.GetCategoryCountAsync();
            var allOrders = await _orderRepository.GetAllOrdersAsync();
            var recentOrders = await _orderRepository.GetRecentOrdersAsync(5);
            var topProducts = await _productRepository.GetTopSellingProductsAsync(5);

            var viewModel = new AdminDashboardViewModel
            {
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalOrders = allOrders.Count,
                TotalRevenue = allOrders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount),
                PendingOrders = allOrders.Count(o => o.Status == OrderStatus.Pending),
                RecentOrders = recentOrders.Take(5).ToList(),
                TopSellingProducts = topProducts.Take(5).ToList()
            };

            return View(viewModel);
        }
    }
}
