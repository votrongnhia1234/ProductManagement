using ProductManagement.Models;

namespace ProductManagement.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync(string? searchTerm = null, OrderStatus? statusFilter = null, int page = 1, int pageSize = 20);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<int> GetOrderCountAsync(string? searchTerm = null, OrderStatus? statusFilter = null);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task<List<Order>> GetRecentOrdersAsync(int count = 10);
        Task<List<Order>> GetAllOrdersAsync(); // For statistics
    }
}
