using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using ProductManagement.Repositories;
using ProductManagement.Models;
using ProductManagement.Areas.Customer.Models;
using System.Text.Json;
using ProductManagement.Services;
using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class StripeController : Controller
    {
        private readonly EmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public StripeController(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            EmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _emailService = emailService;
            _userManager = userManager;
        }

        public IActionResult Index(decimal amount)
        {
            // Lấy PublishableKey từ biến môi trường để truyền cho view nếu cần
            ViewBag.Amount = amount;
            ViewBag.StripePublishableKey = Environment.GetEnvironmentVariable("PublishableKey");
            return View();
        }

        [HttpPost]
        public IActionResult CreateCheckoutSession(decimal amount)
        {
            // Lấy SecretKey từ biến môi trường
            var stripeSecretKey = Environment.GetEnvironmentVariable("SecretKey");
            StripeConfiguration.ApiKey = stripeSecretKey;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(amount * 100), // Stripe dùng đơn vị cent
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Thanh toán đơn hàng"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Stripe", null, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Stripe", null, Request.Scheme)
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        public async Task<IActionResult> Success()
        {
            // Lấy lại thông tin đơn hàng từ TempData
            if (TempData["CheckoutModel"] is string checkoutJson)
            {
                var model = JsonSerializer.Deserialize<CheckoutViewModel>(checkoutJson);

                // Lấy lại danh sách id sản phẩm đã chọn từ session
                var selectedIdsStr = HttpContext.Session.GetString("SelectedCartIds");
                var ids = selectedIdsStr?.Split(',').Select(int.Parse).ToList() ?? new List<int>();
                var cart = GetCartFromSession();
                var selectedItems = cart.Where(i => ids.Contains(i.ProductId)).ToList();

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Tạo order items và tính tổng tiền
                var orderItems = new List<OrderItem>();
                decimal totalAmount = 0;
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

                await _orderRepository.CreateOrderAsync(order);// ...sau khi await _orderRepository.CreateOrderAsync(order);

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        var subject = "Xác nhận đơn hàng";
                        var body = $@"
                        <div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;border:1px solid #eee;padding:24px;background:#fafbfc;'>
                            <h2 style='color:#2b6cb0;text-align:center;margin-bottom:24px;'>Xác nhận đơn hàng</h2>
                            <p style='font-size:16px;'>Xin chào <b>{user.UserName}</b>,</p>
                            <p style='font-size:16px;'>Cảm ơn quý khách đã đặt hàng tại <b>Shop</b>!</p>
                            <table style='width:100%;border-collapse:collapse;margin:16px 0;'>
                                <tr>
                                    <td style='padding:8px 0;font-weight:bold;'>Mã đơn hàng:</td>
                                    <td style='padding:8px 0;color:#2b6cb0;'>{order.Id}</td>
                                </tr>
                                <tr>
                                    <td style='padding:8px 0;font-weight:bold;'>Ngày đặt:</td>
                                    <td style='padding:8px 0;'>{order.OrderDate:dd/MM/yyyy HH:mm}</td>
                                </tr>
                                <tr>
                                    <td style='padding:8px 0;font-weight:bold;'>Tổng tiền:</td>
                                    <td style='padding:8px 0;color:#e53e3e;font-weight:bold;'>$ {order.TotalAmount:N0}</td>
                                </tr>
                                <tr>
                                    <td style='padding:8px 0;font-weight:bold;'>Địa chỉ giao hàng:</td>
                                    <td style='padding:8px 0;'>{order.ShippingAddress}</td>
                                </tr>
                                <tr>
                                    <td style='padding:8px 0;font-weight:bold;'>Phương thức thanh toán:</td>
                                    <td style='padding:8px 0;'>Chuyển khoản ngân hàng</td>
                                </tr>
                                <tr>
                                    <td style='padding:8px 0;font-weight:bold;'>Ghi chú đơn hàng:</td>
                                    <td style='padding:8px 0;'>{order.Notes}</td>
                                </tr>
                            </table>
                            <div style='margin:16px 0;'>
                                <span style='font-weight:bold;'>Danh sách sản phẩm:</span>
                                <ul style='margin:8px 0 16px 20px;padding:0;font-size:15px;'>
                                    {string.Join("", order.OrderItems.Select(item => $"<li>{item.Product.ProductName} x {item.Quantity} = <b>$ {item.Price * item.Quantity:N0}</b></li>"))}
                                </ul>
                            </div>
                            <div style='margin-top:24px;font-size:15px;'>
                                <b>Chúng tôi sẽ liên hệ khi đơn hàng được giao.<br/>Xin cảm ơn quý khách!</b>
                            </div>
                            <hr style='margin:32px 0 8px 0;border:none;border-top:1px solid #eee;'/>
                            <div style='text-align:center;color:#888;font-size:13px;'>TechnoShop - Hotline: 0123 456 789</div>
                        </div>
                        ";
                        await _emailService.SendEmailAsync(user.Email, subject, body);
                    }
                }

                // Xóa các sản phẩm đã thanh toán khỏi giỏ hàng
                var remainCart = cart.Where(i => !ids.Contains(i.ProductId)).ToList();
                SaveCartToSession(remainCart);
                HttpContext.Session.Remove("SelectedCartIds");

                // Tạo ViewModel xác nhận
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

                // Trả về view xác nhận đơn hàng (dùng lại view của Cart)
                return View("~/Areas/Customer/Views/Cart/Confirmation.cshtml", confirmationViewModel);
            }

            // Nếu không có thông tin, chuyển về trang chủ hoặc báo lỗi
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult Cancel()
        {
            ViewBag.Message = "Thanh toán bị hủy!";
            return View();
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