using Mango.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Services
{
    public interface ICouponService: IBaseService
    {
        Task<T?> GetCoupon<T>(string token, string couponCode);
    }
}
