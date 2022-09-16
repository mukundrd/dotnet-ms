using Mango.Contracts.Dtos;

namespace Mango.Services.Coupons.Repository
{
    public interface ICouponRepository
    {

        Task<CouponDto> GetCouponByCode(string couponCode); 

    }
}
