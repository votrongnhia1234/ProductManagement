using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Repositories;
using ProductManagement.Areas.Customer.Models;

namespace ProductManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "" });

            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var viewModel = new CustomerOrderListViewModel
            {
                Orders = orders.Select(o => new CustomerOrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    ItemCount = o.OrderItems.Count,
                    TrackingNumber = o.TrackingNumber
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || order.UserId != user.Id)
            {
                return Forbid();
            }

            var viewModel = new CustomerOrderDetailsViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Notes = order.Notes,
                ShippingAddress = order.ShippingAddress,
                TrackingNumber = order.TrackingNumber,
                ShippedDate = order.ShippedDate,
                DeliveredDate = order.DeliveredDate,
                OrderItems = order.OrderItems.Select(oi => new CustomerOrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.ProductName,
                    ProductImage = oi.Product.ImgUrl,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || order.UserId != user.Id)
            {
                return Forbid();
            }

            // Only allow cancellation of pending or confirmed orders
            if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
            {
                TempData["Error"] = "Không thể hủy đơn hàng này.";
                return RedirectToAction(nameof(Details), new { id });
            }

            order.Status = OrderStatus.Cancelled;
            await _orderRepository.UpdateOrderAsync(order);

            TempData["Success"] = "Đơn hàng đã được hủy thành công.";
            return RedirectToAction(nameof(MyOrders));
        }

        public async Task<IActionResult> MyOrders(int page = 1)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "" });

            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            // Sắp xếp: Pending đầu, Cancelled cuối, còn lại theo ngày giảm dần
            var sortedOrders = orders
                .OrderBy(o => o.Status == OrderStatus.Cancelled ? 2 : (o.Status == OrderStatus.Pending ? 0 : 1))
                .ThenByDescending(o => o.Status == OrderStatus.Pending ? o.OrderDate : DateTime.MinValue)
                .ThenByDescending(o => o.OrderDate)
                .ToList();

            // Phân trang nếu cần
            int pageSize = 10;
            var pagedOrders = sortedOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new CustomerOrderListViewModel
            {
                Orders = pagedOrders.Select(o => new CustomerOrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    ItemCount = o.OrderItems.Count,
                    TrackingNumber = o.TrackingNumber
                }).ToList(),
                TotalOrders = orders.Count,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(orders.Count / (double)pageSize)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || order.UserId != user.Id)
            {
                return Forbid();
            }

            // Chỉ cho phép xóa đơn đã hủy
            if (order.Status != OrderStatus.Cancelled)
            {
                TempData["Error"] = "Chỉ có thể xóa đơn hàng đã hủy.";
                return RedirectToAction(nameof(MyOrders));
            }

            await _orderRepository.DeleteOrderAsync(order.Id);

            TempData["Success"] = "Đơn hàng đã được xóa khỏi danh sách.";
            return RedirectToAction(nameof(MyOrders));
        }
    }
}
