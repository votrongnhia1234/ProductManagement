using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using ProductManagement.Repositories;
using ProductManagement.Models;
using ProductManagement.Areas.Customer.Models;
using System.Text.Json;

namespace ProductManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class StripeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public StripeController(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public IActionResult Index(decimal amount)
        {
            ViewBag.Amount = amount;
            return View();
        }

        [HttpPost]
        public IActionResult CreateCheckoutSession(decimal amount)
        {
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

                await _orderRepository.CreateOrderAsync(order);

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
                // Hoặc nếu bạn có view confirmationCheckout:
                // return View("~/Areas/Customer/Views/Cart/ConfirmationCheckout.cshtml", confirmationViewModel);
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