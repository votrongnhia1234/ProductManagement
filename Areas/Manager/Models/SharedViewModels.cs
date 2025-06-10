using ProductManagement.Models;

namespace ProductManagement.Areas.Manager.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }
    }

    public class TopSellingProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }
} 