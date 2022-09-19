using Mango.Contracts.Dtos;
using Mango.Contracts.Models.Service;

namespace Mango.Services.Orders.Repository
{
    public interface IOrderRepository
    {

        Task<bool> AddOrder(OrderHeader orderHeader);

        Task<bool> UpdateOrderStatusPaymentStatus(int orderId, bool paid);

    }
}
