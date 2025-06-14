using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Areas.Admin.Models;

namespace ProductManagement.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EFOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync(string? searchTerm = null, OrderStatus? statusFilter = null, int page = 1, int pageSize = 20)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o => o.User.FirstName.Contains(searchTerm) ||
                                        o.User.LastName.Contains(searchTerm) ||
                                        o.User.Email.Contains(searchTerm) ||
                                        o.TrackingNumber!.Contains(searchTerm));
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(o => o.Status == statusFilter.Value);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> GetOrderCountAsync(string? searchTerm = null, OrderStatus? statusFilter = null)
        {
            var query = _context.Orders.Include(o => o.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o => o.User.FirstName.Contains(searchTerm) ||
                                        o.User.LastName.Contains(searchTerm) ||
                                        o.User.Email.Contains(searchTerm) ||
                                        o.TrackingNumber!.Contains(searchTerm));
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(o => o.Status == statusFilter.Value);
            }

            return await query.CountAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetRecentOrdersAsync(int count = 10)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public Task DeleteOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsTrackingNumberAsync(string trackingNumber)
        {
            return await _context.Orders.AnyAsync(o => o.TrackingNumber == trackingNumber);
        }
        public async Task<List<AdminOrderExportViewModel>> GetOrdersForAdminAsync(string searchTerm, OrderStatus? statusFilter)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o => o.User.FullName.Contains(searchTerm) || o.User.Email.Contains(searchTerm));
            }
            if (statusFilter.HasValue)
            {
                query = query.Where(o => o.Status == statusFilter.Value);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new AdminOrderExportViewModel
                {
                    Id = o.Id,
                    CustomerName = o.User.FullName,
                    CustomerEmail = o.User.Email,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    TrackingNumber = o.TrackingNumber
                })
                .ToListAsync();
        }
    }
}
