using Mango.Contracts.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var accessToken = await GetAccessToken();
            var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(accessToken, GetUserid());

            CartDto cartDto;
            if (response?.IsSuccess == true)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            else
            {
                cartDto = new CartDto();
            }

            if (cartDto.CartHeader != null)
            {

                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon<ResponseDto>(accessToken, cartDto.CartHeader.CouponCode);
                    if (coupon?.IsSuccess == true)
                    {
                        var couponObj = Deserialize<CouponDto>(coupon.Result);
                        if (couponObj != null)
                        {
                            cartDto.CartHeader.DiscountTotal = couponObj.DiscountAmount;
                        }
                    }

                }

                foreach (var detail in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }
                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
            }

            return cartDto;
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var response = await _cartService.RemoveFromCartAsync<ResponseDto>(await GetAccessToken(), cartDetailsId);

            if (response?.IsSuccess == true)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var response = await _cartService.ApplyCoupon<ResponseDto>(await GetAccessToken(), cartDto);

            if (response?.IsSuccess == true)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var response = await _cartService.RemoveCoupon<ResponseDto>(await GetAccessToken(), cartDto.CartHeader.UserId);

            if (response?.IsSuccess == true)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var respone = await _cartService.Checkout<ResponseDto>(await GetAccessToken(), cartDto.CartHeader);
                if (!respone.IsSuccess)
                {
                    ViewBag.Error = respone.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }
                return RedirectToAction(nameof (Confirmation));
            }
            catch
            {
                return View(cartDto);
            }
        }

        public async Task<IActionResult> Confirmation()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }


    }
}
