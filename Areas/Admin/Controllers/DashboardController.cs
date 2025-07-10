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

            // Tính doanh thu theo từng tháng trong năm hiện tại
            var now = DateTime.Now;
            var monthlyRevenue = new List<decimal>();
            for (int month = 1; month <= 12; month++)
            {
                var monthRevenue = allOrders
                    .Where(o => o.Status == OrderStatus.Delivered && o.OrderDate.Year == now.Year && o.OrderDate.Month == month)
                    .Sum(o => o.TotalAmount);
                monthlyRevenue.Add(monthRevenue);
            }

            // Thống kê trạng thái đơn hàng
            var orderStatusStats = allOrders
                .GroupBy(o => o.Status)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            var viewModel = new AdminDashboardViewModel
            {
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalOrders = allOrders.Count,
                TotalRevenue = allOrders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount),
                PendingOrders = allOrders.Count(o => o.Status == OrderStatus.Pending),
                RecentOrders = recentOrders.Take(5).ToList(),
                TopSellingProducts = topProducts.Take(5).ToList(),
                MonthlyRevenue = monthlyRevenue, // Doanh thu theo tháng
                OrderStatusStats = orderStatusStats // Thống kê trạng thái đơn hàng
            };

            return View(viewModel);
        }
    }
}
