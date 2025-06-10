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
            return RedirectToAction(nameof(Index));
        }
    }
}
