using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Areas.Manager.Models;

namespace ProductManagement.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ManagerDashboardViewModel
            {
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
                ProcessingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Processing),
                CompletedOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Delivered),
                TotalRevenue = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .SumAsync(o => o.TotalAmount),
                RecentOrders = await _context.Orders
                    .Include(o => o.User)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        UserName = o.User.UserName,
                        OrderDate = o.OrderDate,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status
                    })
                    .ToListAsync(),
                TopSellingProducts = await _context.OrderItems
                    .Include(oi => oi.Product)
                    .GroupBy(oi => oi.Product)
                    .Select(g => new TopSellingProductViewModel
                    {
                        ProductId = g.Key.Id,
                        ProductName = g.Key.ProductName,
                        TotalQuantity = g.Sum(oi => oi.Quantity),
                        TotalRevenue = g.Sum(oi => oi.Quantity * oi.Price)
                    })
                    .OrderByDescending(p => p.TotalQuantity)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}
