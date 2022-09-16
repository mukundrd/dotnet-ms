using AutoMapper;
using Mango.Contracts.Dtos;
using Mango.Contracts.Models.Service;
using Mango.Services.Coupons.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Coupons.Repository
{
    public class CouponRepository : ICouponRepository
    {

        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            Coupon coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode == couponCode);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}
