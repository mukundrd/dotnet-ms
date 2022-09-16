using AutoMapper;
using Mango.Services.ShoppingCart.Contexts;
using Mango.Contracts.Dtos;
using Mango.Contracts.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCart.Repositories
{
    public class CartRepository : ICartRepository
    {

        private readonly ApplicationDBContext _db;

        private readonly IMapper _mapper;

        public CartRepository(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);

            CartDetail cartDetails = cart.CartDetails.FirstOrDefault();

            var prodInDb = await _db.Products.FirstOrDefaultAsync(
                u => u.ProductId == cartDetails.ProductId);
            if (prodInDb == null)
            {
                _db.Products.Add(cartDetails.Product);
                await _db.SaveChangesAsync();
            }

            var cartHeaderFromDB = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDB == null)
            {
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                cartDetails.CartHeaderId = cart.CartHeader.CartHeaderId;
                cartDetails.Product = null;
                _db.CartDetails.Add(cartDetails);
                await _db.SaveChangesAsync();
            }
            else
            {
                var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cartDetails.ProductId &&
                    u.CartHeaderId == cartHeaderFromDB.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    cartDetails.CartHeaderId = cartHeaderFromDB.CartHeaderId;
                    cartDetails.Product = null;
                    _db.CartDetails.Add(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    cartDetails.Count += cartDetailsFromDb.Count;
                    cartDetails.CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cartDetails.CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cartDetails.Product = null;
                    _db.CartDetails.Update(cartDetails);
                    await _db.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> ClearCart(string userId)
        {
            bool cleared = false;

            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);

                if (cartHeaderFromDb != null)
                {
                    _db.CartHeaders.RemoveRange(_db.CartHeaders.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                    _db.CartDetails.RemoveRange(_db.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                    await _db.SaveChangesAsync();
                    cleared = true;
                }
            } catch(Exception ex)
            {
            }

            return cleared;
        }

        public async Task<CartDto> GetCartByUserid(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId)
            };
            cart.CartDetails = _db.CartDetails
                .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId).Include(u=>u.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCart(int cartDetailId)
        {
            bool removed = false;
            try
            {
                CartDetail cartDetail = await _db.CartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailId);

                int totalCount = _db.CartDetails
                    .Where(u => u.CartHeaderId == cartDetail.CartHeaderId).Count();

                _db.Remove(cartDetail);

                if (totalCount == 1)
                {
                    var cartHeader = await _db.CartHeaders
                        .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetail.CartHeaderId);
                    _db.Remove(cartHeader);
                }
                await _db.SaveChangesAsync();

                removed = true;
            }
            catch(Exception ex)
            {
            }
            return removed;
        }

        public async Task<bool> ApplyCoupon(string userId, string CouponCode)
        {
            bool applied = false;
            var cartFromDb = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartFromDb != null)
            {
                cartFromDb.CouponCode = CouponCode;
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                applied = true;
            }

            return applied;
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            bool applied = false;
            var cartFromDb = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartFromDb != null)
            {
                cartFromDb.CouponCode = "";
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                applied = true;
            }

            return applied;

        }
    }
}
