using AutoMapper;
using Mango.Contracts.Dtos;
using Mango.Contracts.Models.Service;

namespace Mango.Services.Coupons
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
