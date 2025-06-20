using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Repositories;
using ProductManagement.Areas.Customer.Models;
using System.Text.Json;
using ProductManagement.Extensions;
using ProductManagement.Services; // Thêm namespace chứa EmailService nếu cần

namespace ProductManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Policy = "RequireCustomerRole")]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService; // Thêm dòng này

        public CartController(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            EmailService emailService // Thêm tham số này
        )
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _emailService = emailService; // Gán vào field
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetCartFromSession();
            var cartItems = new List<CartItemViewModel>();

            foreach (var item in cart)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    cartItems.Add(new CartItemViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        ImgUrl = product.ImgUrl,
                        TotalPrice = product.Price * item.Quantity
                    });
                }
            }

            var viewModel = new CartViewModel
            {
                Items = cartItems,
                TotalAmount = cartItems.Sum(i => i.TotalPrice),
                ItemCount = cartItems.Sum(i => i.Quantity)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCartFromSession();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { ProductId = productId, Quantity = quantity });
            }

            SaveCartToSession(cart);
            TempData["Success"] = $"Đã thêm {product.ProductName} vào giỏ hàng.";
            return RedirectToAction("Index", "Home", new { id = productId });
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveCartToSession(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(cart);
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(string selectedProductIds)
        {
            if (string.IsNullOrEmpty(selectedProductIds))
                selectedProductIds = HttpContext.Session.GetString("SelectedCartIds");

            if (string.IsNullOrEmpty(selectedProductIds))
            {
                TempData["Error"] = "Vui lòng chọn sản phẩm để thanh toán.";
                return RedirectToAction(nameof(Index));
            }

            HttpContext.Session.SetString("SelectedCartIds", selectedProductIds);

            var ids = selectedProductIds.Split(',').Select(int.Parse).ToList();
            var cart = GetCartFromSession();
            var selectedItems = cart.Where(i => ids.Contains(i.ProductId)).ToList();

            if (!selectedItems.Any())
            {
                TempData["Error"] = "Không có sản phẩm hợp lệ để thanh toán.";
                return RedirectToAction(nameof(Index));
            }

            var cartItemViewModels = new List<CartItemViewModel>();
            foreach (var item in selectedItems)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    cartItemViewModels.Add(new CartItemViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        ImgUrl = product.ImgUrl,
                        TotalPrice = product.Price * item.Quantity
                    });
                }
            }

            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItemViewModels,
                TotalAmount = cartItemViewModels.Sum(i => i.TotalPrice)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var selectedIdsStr = HttpContext.Session.GetString("SelectedCartIds");
            if (string.IsNullOrEmpty(selectedIdsStr))
                return RedirectToAction("Index");

            var ids = selectedIdsStr.Split(',').Select(int.Parse).ToList();
            var cart = GetCartFromSession();
            var selectedItems = cart.Where(i => ids.Contains(i.ProductId)).ToList();

            if (!selectedItems.Any())
                return RedirectToAction("Index");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "" });

            var user = await _userManager.FindByIdAsync(userId);

            decimal totalAmount = 0;

            // Nếu chọn PayPal thì chuyển hướng sang trang Payment
            if (model.PaymentMethod == "PayPal")
            {
                foreach (var item in selectedItems)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        totalAmount += product.Price * item.Quantity;
                    }
                }
                TempData["CheckoutModel"] = System.Text.Json.JsonSerializer.Serialize(model);
                return RedirectToAction("Index", "Payment", new { area = "Customer", amount = totalAmount });
            }

            // Nếu chọn Stripe thì chuyển hướng sang trang Stripe
            if (model.PaymentMethod == "Stripe")
            {
                foreach (var item in selectedItems)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        totalAmount += product.Price * item.Quantity;
                    }
                }
                TempData["CheckoutModel"] = System.Text.Json.JsonSerializer.Serialize(model);
                return RedirectToAction("Index", "Stripe", new { area = "Customer", amount = totalAmount });
            }

            // Xử lý đơn hàng bình thường (COD)
            var orderItems = new List<OrderItem>();
            foreach (var item in selectedItems)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    orderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                    totalAmount += product.Price * item.Quantity;
                }
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = totalAmount,
                ShippingAddress = model.ShippingAddress,
                Notes = model.Notes,
                OrderItems = orderItems
            };

            await _orderRepository.CreateOrderAsync(order);

            // Gửi email xác nhận đơn hàng
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                var subject = "Xác nhận đơn hàng";
                var body = $@"
                <div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;border:1px solid #eee;padding:24px;background:#fafbfc;'>
                    <h2 style='color:#2b6cb0;text-align:center;margin-bottom:24px;'>Xác nhận đơn hàng</h2>
                    <p style='font-size:16px;'>Xin chào <b>{user.FullName}</b>,</p>
                    <p style='font-size:16px;'>Cảm ơn quý khách đã đặt hàng tại <b>TechnoShop</b>!</p>
                    <table style='width:100%;border-collapse:collapse;margin:16px 0;'>
                        <tr>
                            <td style='padding:8px 0;font-weight:bold;'>Mã đơn hàng:</td>
                            <td style='padding:8px 0;color:#2b6cb0;'>{order.Id}</td>
                        </tr>
                        <tr>
                            <td style='padding:8px 0;font-weight:bold;'>Ngày đặt:</td>
                            <td style='padding:8px 0;'>{order.OrderDate.AddHours(7):dd/MM/yyyy HH:mm}</td>
                        </tr>
                        <tr>
                            <td style='padding:8px 0;font-weight:bold;'>Tổng tiền:</td>
                            <td style='padding:8px 0;color:#e53e3e;font-weight:bold;'>${order.TotalAmount:N0}</td>
                        </tr>
                        <tr>
                            <td style='padding:8px 0;font-weight:bold;'>Địa chỉ giao hàng:</td>
                            <td style='padding:8px 0;'>{order.ShippingAddress}</td>
                        </tr>
                        <tr>
                            <td style='padding:8px 0;font-weight:bold;'>Phương thức thanh toán:</td>
                                    <td style='padding:8px 0;'>Chuyển khoản khi nhận hàng (COD)</td>
                        </tr>
                        <tr>
                            <td style='padding:8px 0;font-weight:bold;'>Ghi chú đơn hàng:</td>
                            <td style='padding:8px 0;'>{order.Notes}</td>
                        </tr>
                    </table>
                    <div style='margin:16px 0;'>
                        <span style='font-weight:bold;'>Danh sách sản phẩm:</span>
                        <ul style='margin:8px 0 16px 20px;padding:0;font-size:15px;'>
                            {string.Join("", order.OrderItems.Select(item => $"<li>{item.Product.ProductName} x {item.Quantity} = <b>${item.Price * item.Quantity:N0}</b></li>"))}
                        </ul>
                    </div>
                    <div style='margin-top:24px;font-size:15px;'>
                        <b>Chúng tôi sẽ liên hệ khi đơn hàng được giao.<br/>Xin cảm ơn quý khách!</b>
                    </div>
                    <hr style='margin:32px 0 8px 0;border:none;border-top:1px solid #eee;'/>
                    <div style='text-align:center;color:#888;font-size:13px;'>Shop - Hotline: 0123 456 789</div>
                </div>
                ";
                await _emailService.SendEmailAsync(user.Email, subject, body);
            }

            // Xóa các sản phẩm đã thanh toán khỏi giỏ hàng
            var remainCart = cart.Where(i => !ids.Contains(i.ProductId)).ToList();
            SaveCartToSession(remainCart);
            HttpContext.Session.Remove("SelectedCartIds");

            var confirmationViewModel = new OrderConfirmationViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                Items = order.OrderItems.Select(item => new OrderItemViewModel
                {
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.Price * item.Quantity
                }).ToList()
            };

            return View("Confirmation", confirmationViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ExportInvoice(int orderId, [FromServices] OrderInvoicePdfService pdfService)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound();

            // Lấy thông tin user
            var user = await _userManager.FindByIdAsync(order.UserId);

            // Map sang OrderConfirmationViewModel và truyền thêm thông tin khách hàng qua ViewBag
            var orderVm = new OrderConfirmationViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate.AddHours(7),
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                Items = order.OrderItems.Select(item => new OrderItemViewModel
                {
                    ProductName = item.Product?.ProductName ?? "",
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.Price * item.Quantity
                }).ToList()
            };

            // Truyền thông tin khách hàng qua ViewBag hoặc một object phụ
            var customerName = user?.FullName ?? "Khách hàng";
            var customerEmail = user?.Email ?? "N/A";

            var pdfBytes = pdfService.GenerateInvoice(orderVm, customerName, customerEmail);
            return File(pdfBytes, "application/pdf", $"HoaDon_{orderVm.OrderId}.pdf");
        }

        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}

// Đặt class CartItem ở ngoài namespace để các controller khác dùng chung
public class CartItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
