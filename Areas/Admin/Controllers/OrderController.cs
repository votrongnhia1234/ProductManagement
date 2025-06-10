using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Repositories;
using ProductManagement.Areas.Admin.Models;
using ProductManagement.Models;

namespace ProductManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")]
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

            // Calculate statistics
            var allOrders = await _orderRepository.GetAllOrdersAsync();
            var stats = new OrderStatisticsViewModel
            {
                TotalOrders = allOrders.Count,
                PendingOrders = allOrders.Count(o => o.Status == OrderStatus.Pending),
                ProcessingOrders = allOrders.Count(o => o.Status == OrderStatus.Processing),
                ShippedOrders = allOrders.Count(o => o.Status == OrderStatus.Shipped),
                DeliveredOrders = allOrders.Count(o => o.Status == OrderStatus.Delivered),
                CancelledOrders = allOrders.Count(o => o.Status == OrderStatus.Cancelled),
                TotalRevenue = allOrders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount),
                AverageOrderValue = allOrders.Any() ? allOrders.Average(o => o.TotalAmount) : 0
            };

            var viewModel = new AdminOrderManagementViewModel
            {
                Orders = orders.Select(o => new AdminOrderViewModel
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
                Statistics = stats,
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

            var viewModel = new AdminOrderDetailsViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Customer = new CustomerInfoViewModel
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
                OrderItems = order.OrderItems.Select(oi => new AdminOrderItemViewModel
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
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status, string? trackingNumber = null, string? notes = null)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var oldStatus = order.Status;
            order.Status = status;

            if (!string.IsNullOrEmpty(notes))
            {
                order.Notes = notes;
            }

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

            TempData["Success"] = $"Đơn hàng đã được cập nhật từ {GetStatusText(oldStatus)} thành {GetStatusText(status)}.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Only allow deletion of cancelled orders
            if (order.Status != OrderStatus.Cancelled)
            {
                TempData["Error"] = "Chỉ có thể xóa đơn hàng đã hủy.";
                return RedirectToAction(nameof(Details), new { id });
            }

            await _orderRepository.DeleteOrderAsync(id);
            TempData["Success"] = "Đơn hàng đã được xóa thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Export(string searchTerm, OrderStatus? statusFilter)
        {
            var orders = await _orderRepository.GetOrdersAsync(searchTerm, statusFilter, 1, int.MaxValue);

            // Simple CSV export
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("ID,Ngày đặt,Khách hàng,Email,Tổng tiền,Trạng thái,Địa chỉ giao hàng");

            foreach (var order in orders)
            {
                csv.AppendLine($"{order.Id},{order.OrderDate:dd/MM/yyyy},{order.User.FullName},{order.User.Email},{order.TotalAmount},{GetStatusText(order.Status)},{order.ShippingAddress}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"orders_{DateTime.Now:yyyyMMdd}.csv");
        }

        private string GetStatusText(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Chờ xử lý",
                OrderStatus.Confirmed => "Đã xác nhận",
                OrderStatus.Processing => "Đang xử lý",
                OrderStatus.Shipped => "Đã gửi hàng",
                OrderStatus.Delivered => "Đã giao hàng",
                OrderStatus.Cancelled => "Đã hủy",
                OrderStatus.Returned => "Đã trả hàng",
                _ => "Không xác định"
            };
        }
    }
}
