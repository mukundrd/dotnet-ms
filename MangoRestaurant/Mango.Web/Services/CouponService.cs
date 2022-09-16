using Mango.Contracts.Dtos;

namespace Mango.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {

        public CouponService(IHttpClientFactory clientFactory) : base(clientFactory) { }

        public async Task<T?> GetCoupon<T>(string token, string couponCode)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = couponCode,
                Url = SD.CouponAPIBase + "/api/coupons/" + couponCode,
                AccessToken = token
            });
        }
    }
}
