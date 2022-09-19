using Mango.Contracts.Models.Service;
using Mango.Services.Orders.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Orders.Repository
{
    public class OrderRepository : IOrderRepository
    {

        private readonly DbContextOptions<ApplicationDBContext> _dbContext;

        public OrderRepository(DbContextOptions<ApplicationDBContext> dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            bool saved = false;
            try
            {
                await using var _db = new ApplicationDBContext(_dbContext);
                _db.OrderHeaders.Add(orderHeader);
                await _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                saved = false;
            }
            return saved;
        }

        public async Task<bool> UpdateOrderStatusPaymentStatus(int orderId, bool paid)
        {
            bool saved = false;
            try
            {
                await using var _db = new ApplicationDBContext(_dbContext);
                var orderheader = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderId);
                if(orderheader != null)
                {
                    orderheader.PaymentStatus = paid;
                }
                await _db.SaveChangesAsync();
                saved = true;
            }
            catch (Exception ex)
            {
                saved = false;
            }
            return saved;
        }
    }
}
