using ProductManagement.Models;
using ProductManagement.Areas.Manager.Models; // For shared OrderViewModel

namespace ProductManagement.Areas.Manager.Models
{
    public class OrderManagementViewModel
    {
        public List<OrderViewModel> Orders { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public OrderStatus? StatusFilter { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalOrders { get; set; }
    }

    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerViewModel Customer { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new();
    }

    public class CustomerViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
