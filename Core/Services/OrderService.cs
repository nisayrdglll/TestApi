using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Models;
using DataAccess.DTOs;

namespace Core.Services
{
    public class OrderService
    {
        private readonly DbContext _context;

        public OrderService(DbContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderRequestDTO> CreateOrderAsync(CreateOrderRequestDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var productIds = request.OrderItems.Select(x => x.ProductId).ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id) && p.Deleted == 0 && p.IsActive)
                    .ToListAsync();

                if (products.Count != productIds.Count)
                {
                    throw new ArgumentException("Bazı ürünler bulunamadı veya aktif değil.");
                }

                decimal totalAmount = 0;
                var orderItems = new List<OrderResponseDTO>();

                foreach (var item in request.OrderItems)
                {
                    var product = products.First(p => p.Id == item.ProductId);

                    if (product.StockQuantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"'{product.Name}' ürünü için yeterli stok yok. Mevcut stok: {product.StockQuantity}");
                    }

                    product.StockQuantity -= item.Quantity;
                    product.UpdatedAt = DateTime.UtcNow.ToLocalTime();

                    var orderItem = new OrderResponseDTO
                    {
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        TotalPrice = product.Price * item.Quantity,
                        CreatedAt = DateTime.UtcNow.ToLocalTime()
                    };

                    orderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;
                }

                var order = new CreateOrderRequestDTO
                {
                    UserId = request.UserId,
                    OrderDate = DateTime.UtcNow.ToLocalTime(),
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    ShippingAddress = request.ShippingAddress,
                    Notes = request.Notes,
                    CreatedAt = DateTime.UtcNow.ToLocalTime(),
                    OrderItems = orderItems
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetOrderByIdWithDetailsAsync(order.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderListResponseDTO>> GetOrdersByUserIdAsync(string userId, int page = 1, int limit = 10)
        {
            var query = _context.Orders
                .Where(o => o.UserId == userId && o.Deleted == 0)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt);

            var orders = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(o => new OrderListResponseDTO
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    ItemCount = o.OrderItems.Count(oi => oi.Deleted == 0)
                })
                .ToListAsync();

            return orders;
        }

        public async Task<OrderResponseDTO?> GetOrderDetailsByIdAsync(long orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems.Where(oi => oi.Deleted == 0))
                .Where(o => o.Id == orderId && o.Deleted == 0)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            return new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };
        }

        public async Task<bool> DeleteOrderAsync(long orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.Id == orderId && o.Deleted == 0)
                    .FirstOrDefaultAsync();

                if (order == null)
                    return false;

                if (order.Status != "Pending")
                {
                    throw new InvalidOperationException("Sadece beklemede olan siparişler silinebilir.");
                }

                var productIds = order.OrderItems.Select(oi => oi.ProductId).ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var orderItem in order.OrderItems)
                {
                    var product = products.First(p => p.Id == orderItem.ProductId);
                    product.StockQuantity += orderItem.Quantity;
                    product.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                }

                order.Deleted = 1;
                order.DeletedAt = DateTime.UtcNow.ToLocalTime();

                foreach (var orderItem in order.OrderItems)
                {
                    orderItem.Deleted = 1;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> GetOrderCountByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.Deleted == 0)
                .CountAsync();
        }

        private async Task<CreateOrderRequestDTO> GetOrderByIdWithDetailsAsync(long orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstAsync(o => o.Id == orderId);
        }
    }
}