using Mango.Contracts.Dtos;
using Mango.Services.Coupons.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Coupons.Controllers
{
    [ApiController]
    [Route("api/coupons")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        private ResponseDto _response = new();

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet("{couponCode}")]
        public async Task<object> GetCoupon(string couponCode)
        {
            try
            {
                _response.Result = await _couponRepository.GetCouponByCode(couponCode);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
