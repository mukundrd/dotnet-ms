using AutoMapper;
using Mango.Contracts.Dtos;
using Mango.Contracts.Messages;
using Mango.Contracts.Models.Service;

namespace Mango.Services.Orders
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeader, CheckoutHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetail, CartDetailDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
