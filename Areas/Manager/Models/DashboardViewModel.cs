using ProductManagement.Models;
using System;
using System.Collections.Generic;

namespace ProductManagement.Areas.Manager.Models
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }
        public List<OrderViewModel> RecentOrders { get; set; } = new();
    }
} 