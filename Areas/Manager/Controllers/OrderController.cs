using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Areas.Customer.Models;
using ProductManagement.Areas.Manager.Models;
using ProductManagement.Models;
using ProductManagement.Repositories;

namespace ProductManagement.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "RequireManagerRole")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index(string searchTerm, OrderStatus? statusFilter, int page = 1)
        {
            const int pageSize = 20;

            var orders = await _orderRepository.GetOrdersAsync(searchTerm, statusFilter, page, pageSize);
            var totalOrders = await _orderRepository.GetOrderCountAsync(searchTerm, statusFilter);
            var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            var viewModel = new OrderManagementViewModel
            {
                Orders = orders.Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerName = o.User.FullName,
                    CustomerEmail = o.User.Email!,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    ItemCount = o.OrderItems.Count,
                    ShippingAddress = o.ShippingAddress,
                    TrackingNumber = o.TrackingNumber
                }).ToList(),
                SearchTerm = searchTerm ?? string.Empty,
                StatusFilter = statusFilter,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalOrders = totalOrders
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

            var viewModel = new OrderDetailsViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Customer = new CustomerViewModel
                {
                    Id = order.User.Id,
                    FullName = order.User.FullName,
                    Email = order.User.Email!,
                    Address = order.User.Address
                },
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Notes = order.Notes,
                ShippingAddress = order.ShippingAddress,
                TrackingNumber = order.TrackingNumber,
                ShippedDate = order.ShippedDate,
                DeliveredDate = order.DeliveredDate,
                OrderItems = order.OrderItems.Select(oi => new ProductManagement.Areas.Manager.Models.OrderItemViewModel
                {
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
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status, string? trackingNumber = null)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;

            if (status == OrderStatus.Shipped && !string.IsNullOrEmpty(trackingNumber))
            {
                order.TrackingNumber = trackingNumber;
                order.ShippedDate = DateTime.UtcNow;
            }
            else if (status == OrderStatus.Delivered)
            {
                order.DeliveredDate = DateTime.UtcNow;
            }

            await _orderRepository.UpdateOrderAsync(order);

            TempData["Success"] = "Trạng thái đơn hàng đã được cập nhật.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
