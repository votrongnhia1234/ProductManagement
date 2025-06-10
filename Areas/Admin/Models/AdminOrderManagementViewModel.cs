using ProductManagement.Models;

namespace ProductManagement.Areas.Admin.Models
{
    public class AdminOrderManagementViewModel
    {
        public List<AdminOrderViewModel> Orders { get; set; } = new();
        public OrderStatisticsViewModel Statistics { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public OrderStatus? StatusFilter { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalOrders { get; set; }
    }

    public class AdminOrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }
    }

    public class AdminOrderDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerInfoViewModel Customer { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public List<AdminOrderItemViewModel> OrderItems { get; set; } = new();
    }

    public class CustomerInfoViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public class AdminOrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderStatisticsViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
