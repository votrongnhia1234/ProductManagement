using ProductManagement.Models;
using ProductManagement.Areas.Manager.Models; // For shared view models

namespace ProductManagement.Areas.Manager.Models
{
    public class ManagerDashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<OrderViewModel> RecentOrders { get; set; } = new();
        public List<TopSellingProductViewModel> TopSellingProducts { get; set; } = new();
    }
}
