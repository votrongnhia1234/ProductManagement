using ProductManagement.Models;

namespace ProductManagement.Areas.Admin.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders { get; set; }
        public List<Order> RecentOrders { get; set; } = new();
        public List<Product> TopSellingProducts { get; set; } = new();
    }
} 