using Mango.Contracts.Dtos;
using Mango.Contracts.Messages;
using Mango.MessageBus.Producers.Messages;
using Mango.Services.ShoppingCart.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCart.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        private readonly ICouponRepository _couponRepository;

        private readonly IMessageBus _messageBus;

        private ResponseDto _response = new();

        public CartController(ICartRepository cartRepository, IMessageBus messageBus, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _messageBus = messageBus;
            _couponRepository = couponRepository;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                _response.Result = await _cartRepository.GetCartByUserid(userId);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cart)
        {
            try
            {
                _response.Result = await _cartRepository.CreateUpdateCart(cart);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDto cart)
        {
            try
            {
                _response.Result = await _cartRepository.CreateUpdateCart(cart);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartDetailId)
        {
            try
            {
                _response.Result = await _cartRepository.RemoveFromCart(cartDetailId);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("ClearCart")]
        public async Task<object> ClearCart([FromBody] string userId)
        {
            try
            {
                _response.Result = await _cartRepository.ClearCart(userId);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                _response.Result = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                _response.Result = await _cartRepository.RemoveCoupon(userId);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout([FromBody] CheckoutHeaderDto checkoutHead)
        {
            try
            {

                if (!string.IsNullOrEmpty(checkoutHead.CouponCode))
                {
                    CouponDto coupon = await _couponRepository.GetCoupon(checkoutHead.CouponCode);
                    if (coupon == null)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>()
                        {
                            "Coupon doesn't exist or invalid, please confirm"
                        };
                        _response.DisplayMessage = "Coupon doesn't exist or invalid, please confirm";
                        return _response;
                    }
                    else if (checkoutHead.DiscountTotal != coupon.DiscountAmount)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>()
                        {
                            "Coupon Price has changed, please confirm"
                        };
                        _response.DisplayMessage = "Coupon Price has changed, please confirm";
                        return _response;
                    }
                }

                var cartDto = await _cartRepository.GetCartByUserid(checkoutHead.UserId);
                if (cartDto == null)
                {
                    return BadRequest();
                }

                checkoutHead.CartDetails = cartDto.CartDetails;
                await _messageBus.PublishMessage(checkoutHead, "checkoutmessagetopic");

                await _cartRepository.ClearCart(checkoutHead.UserId);

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
