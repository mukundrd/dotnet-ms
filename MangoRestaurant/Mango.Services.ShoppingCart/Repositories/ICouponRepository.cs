using Mango.Contracts.Dtos;

namespace Mango.Services.ShoppingCart.Repositories
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
