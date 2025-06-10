using ProductManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Areas.Customer.Models
{
    public class CustomerHomeViewModel
    {
        public List<CustomerProductViewModel> Products { get; set; } = new();
        public List<CustomerProductViewModel> FeaturedProducts { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public int? CategoryFilter { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalProducts { get; set; }
    }

    public class CustomerProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CustomerProductDetailsViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public List<CustomerProductViewModel> RelatedProducts { get; set; } = new();
    }

    public class CustomerOrderListViewModel
    {
        public List<CustomerOrderViewModel> Orders { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalOrders { get; set; }
    }

    public class CustomerOrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
        public string? TrackingNumber { get; set; }
    }

    public class CustomerOrderDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public List<CustomerOrderItemViewModel> OrderItems { get; set; } = new();
    }

    public class CustomerOrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderConfirmationViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItemViewModel> Items { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
