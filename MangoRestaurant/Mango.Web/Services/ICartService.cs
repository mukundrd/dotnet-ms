using Mango.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Services
{
    public interface ICartService: IBaseService
    {
        Task<T?> GetCartByUserIdAsync<T>(string token, string userid);

        Task<T?> AddToCartAsync<T>(string token, CartDto cart);

        Task<T?> UpdateCartAsync<T>(string token, CartDto cart);

        Task<T?> RemoveFromCartAsync<T>(string token, int cartDetailId);

        Task<T?> ClearCartFOrUserAsync<T>(string token, string userId);

        Task<T?> ApplyCoupon<T>(string token, CartDto cartDto);

        Task<T?> RemoveCoupon<T>(string token, string userId);

        Task<T?> Checkout<T>(string token, CartHeaderDto cartHeaderDto);
    }
}
