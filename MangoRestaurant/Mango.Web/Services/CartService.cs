using Mango.Contracts.Dtos;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {

        public CartService(IHttpClientFactory clientFactory) : base(clientFactory) { }

        public async Task<T?> AddToCartAsync<T>(string token, CartDto cartDto)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T?> ClearCartFOrUserAsync<T>(string token, string userId)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = userId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ClearCart",
                AccessToken = token
            });
        }

        public async Task<T?> GetCartByUserIdAsync<T>(string token, string userid)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/cart/getCart/" + userid,
                AccessToken = token
            });
        }

        public async Task<T?> RemoveFromCartAsync<T>(string token, int cartDetailId)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T?> UpdateCartAsync<T>(string token, CartDto cartDto)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/UpdateCart",
                AccessToken = token
            });
        }

        public async Task<T?> ApplyCoupon<T>(string token, CartDto cartDto)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T?> RemoveCoupon<T>(string token, string userId)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = userId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T?> Checkout<T>(string token, CartHeaderDto cartHeaderDto)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartHeaderDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/Checkout",
                AccessToken = token
            });
        }
    }
}
