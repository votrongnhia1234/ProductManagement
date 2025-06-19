using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Repositories;
using ProductManagement.Areas.Admin.Models;
using ProductManagement.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProductManagement.Services;
using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly EmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(
            IOrderRepository orderRepository,
            EmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _emailService = emailService;
            _userManager = userManager;
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateStatus(int id, OrderStatus status, string? trackingNumber = null, string? notes = null)
        //{
        //    var order = await _orderRepository.GetOrderByIdAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    var oldStatus = order.Status;
        //    order.Status = status;

        //    if (!string.IsNullOrEmpty(notes))
        //    {
        //        order.Notes = notes;
        //    }

        //    if (status == OrderStatus.Shipped && !string.IsNullOrEmpty(trackingNumber))
        //    {
        //        order.TrackingNumber = trackingNumber;
        //        order.ShippedDate = DateTime.UtcNow;
        //    }
        //    else if (status == OrderStatus.Delivered)
        //    {
        //        order.DeliveredDate = DateTime.UtcNow;
        //    }

        //    await _orderRepository.UpdateOrderAsync(order);

        //    TempData["Success"] = $"Đơn hàng đã được cập nhật từ {GetStatusText(oldStatus)} thành {GetStatusText(status)}.";
        //    return RedirectToAction(nameof(Details), new { id });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            order.Status = status;

            if (status == OrderStatus.Shipped)
            {
                // Nếu chưa có mã vận đơn thì sinh mới
                if (string.IsNullOrEmpty(order.TrackingNumber))
                {
                    string newTrackingNumber;
                    do
                    {
                        // Ví dụ: SHP + 8 ký tự ngẫu nhiên
                        newTrackingNumber = "SHP" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                    }
                    // Đảm bảo không trùng trong database
                    while (await _orderRepository.ExistsTrackingNumberAsync(newTrackingNumber));

                    order.TrackingNumber = newTrackingNumber;
                }
                order.ShippedDate = DateTime.UtcNow;
            }

            await _orderRepository.UpdateOrderAsync(order);

            // Gửi email khi giao hàng hoặc hủy đơn
            var user = await _userManager.FindByIdAsync(order.UserId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                string subject = "";
                string body = "";

                if (status == OrderStatus.Delivered)
                {
                    subject = "Đơn hàng đã được giao thành công";
                    body = $@"
                        <p>Xin chào <b>{user.UserName}</b>,</p>
                        <p>Đơn hàng <b>#{order.Id}</b> của bạn đã được giao thành công.</p>
                        <ul>
                            <li><b>Ngày giao:</b> {DateTime.Now:dd/MM/yyyy HH:mm}</li>
                            <li><b>Tổng tiền:</b> {order.TotalAmount:C0}</li>
                            <li><b>Địa chỉ nhận hàng:</b> {order.ShippingAddress}</li>
                        </ul>
                        <p>Cảm ơn bạn đã mua hàng tại cửa hàng của chúng tôi!</p>
                    ";
                }
                else if (status == OrderStatus.Cancelled)
                {
                    subject = "Đơn hàng đã bị hủy";
                    body = $@"
                        <p>Xin chào <b>{user.UserName}</b>,</p>
                        <p>Đơn hàng <b>#{order.Id}</b> của bạn đã bị hủy.</p>
                        <p>Nếu có thắc mắc, vui lòng liên hệ cửa hàng để được hỗ trợ.</p>
                    ";
                }

                if (!string.IsNullOrEmpty(subject))
                {
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
            }

            TempData["Success"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction("Details", new { id });
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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var orders = await _orderRepository.GetOrdersForAdminAsync(searchTerm, statusFilter);

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Orders");

                // Header
                ws.Cells[1, 1].Value = "Mã ĐH";
                ws.Cells[1, 2].Value = "Khách hàng";
                ws.Cells[1, 3].Value = "Email";
                ws.Cells[1, 4].Value = "Ngày đặt";
                ws.Cells[1, 5].Value = "Tổng tiền";
                ws.Cells[1, 6].Value = "Trạng thái";
                ws.Cells[1, 7].Value = "Mã vận đơn";

                // Data
                int row = 2;
                foreach (var order in orders)
                {
                    ws.Cells[row, 1].Value = order.Id;
                    ws.Cells[row, 2].Value = order.CustomerName;
                    ws.Cells[row, 3].Value = order.CustomerEmail;
                    ws.Cells[row, 4].Value = order.OrderDate.ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[row, 5].Value = order.TotalAmount;
                    ws.Cells[row, 6].Value = GetStatusText(order.Status);
                    ws.Cells[row, 7].Value = order.TrackingNumber;
                    row++;
                }

                ws.Cells[1, 1, row - 1, 7].AutoFitColumns();
                ws.Cells[1, 1, 1, 7].Style.Font.Bold = true;

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"orders_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        // Hàm chuyển trạng thái sang tiếng Việt (có thể dùng chung với view)
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
