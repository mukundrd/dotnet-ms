using Mango.Contracts.Dtos;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace Mango.Services.ShoppingCart.Repositories
{
    public class CouponRepository : ICouponRepository
    {

        private readonly HttpClient _httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var response = await _httpClient.GetAsync($"api/coupon/" + couponCode);
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (responseDto?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }

            return new CouponDto();
        }
    }
}
