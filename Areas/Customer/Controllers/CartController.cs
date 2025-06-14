using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Repositories;
using ProductManagement.Areas.Customer.Models;
using System.Text.Json;
using ProductManagement.Extensions;

namespace ProductManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Policy = "RequireCustomerRole")]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
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

            return RedirectToAction("ProductDetails", "Home", new { id = productId });
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

        // [HttpGet]
        // public async Task<IActionResult> Checkout()
        // {
        //     var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        //     if (!cartItems.Any())
        //     {
        //         TempData["Error"] = "Giỏ hàng trống, không thể thanh toán.";
        //         return RedirectToAction(nameof(Index));
        //     }

        //     var cartItemViewModels = new List<CartItemViewModel>();
        //     foreach (var item in cartItems)
        //     {
        //         var product = await _productRepository.GetProductByIdAsync(item.ProductId);
        //         if (product != null)
        //         {
        //             cartItemViewModels.Add(new CartItemViewModel
        //             {
        //                 ProductId = product.Id,
        //                 ProductName = product.ProductName,
        //                 Price = product.Price,
        //                 Quantity = item.Quantity,
        //                 TotalPrice = product.Price * item.Quantity
        //             });
        //         }
        //     }

        //     var viewModel = new CheckoutViewModel
        //     {
        //         CartItems = cartItemViewModels,
        //         TotalAmount = cartItemViewModels.Sum(i => i.TotalPrice)
        //     };

        //     return View(viewModel);
        // }

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

        // [HttpPost]
        // [Authorize]
        // public async Task<IActionResult> Checkout(CheckoutViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //         return View(model);

        //     // Lấy cart từ session đúng kiểu
        //     var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        //     if (!cartItems.Any())
        //         return RedirectToAction("Index");

        //     // Map lại sang CartItemViewModel
        //     var cartItemViewModels = new List<CartItemViewModel>();
        //     foreach (var item in cartItems)
        //     {
        //         var product = await _productRepository.GetProductByIdAsync(item.ProductId);
        //         if (product != null)
        //         {
        //             cartItemViewModels.Add(new CartItemViewModel
        //             {
        //                 ProductId = product.Id,
        //                 ProductName = product.ProductName,
        //                 Price = product.Price,
        //                 Quantity = item.Quantity,
        //                 TotalPrice = product.Price * item.Quantity
        //             });
        //         }
        //     }

        //     var cartViewModel = new CartViewModel
        //     {
        //         Items = cartItemViewModels,
        //         TotalAmount = cartItemViewModels.Sum(i => i.TotalPrice),
        //         ItemCount = cartItemViewModels.Sum(i => i.Quantity)
        //     };

        //     var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //     if (string.IsNullOrEmpty(userId))
        //         return RedirectToAction("Login", "Account", new { area = "" });

        //     var order = new Order
        //     {
        //         UserId = userId,
        //         OrderDate = DateTime.UtcNow,
        //         Status = OrderStatus.Pending,
        //         TotalAmount = cartViewModel.TotalAmount,
        //         ShippingAddress = model.ShippingAddress,
        //         Notes = model.Notes,
        //         OrderItems = cartViewModel.Items.Select(item => new OrderItem
        //         {
        //             ProductId = item.ProductId,
        //             Quantity = item.Quantity,
        //             Price = item.Price
        //         }).ToList()
        //     };

        //     await _orderRepository.CreateOrderAsync(order);
        //     HttpContext.Session.Remove("Cart");

        //     var confirmationViewModel = new OrderConfirmationViewModel
        //     {
        //         OrderId = order.Id,
        //         OrderDate = order.OrderDate,
        //         TotalAmount = order.TotalAmount,
        //         Status = order.Status,
        //         ShippingAddress = order.ShippingAddress,
        //         Items = order.OrderItems.Select(item => new OrderItemViewModel
        //         {
        //             ProductName = item.Product.ProductName,
        //             Quantity = item.Quantity,
        //             Price = item.Price,
        //             TotalPrice = item.Price * item.Quantity
        //         }).ToList()
        //     };

        //     return View("Confirmation", confirmationViewModel);
        // }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            // Lấy danh sách id sản phẩm đã chọn từ session
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

            // Tạo đơn hàng chỉ với các sản phẩm đã chọn
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

            // Tạo ViewModel xác nhận chỉ với sản phẩm đã thanh toán
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

    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
