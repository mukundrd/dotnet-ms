using AutoMapper;
using Mango.Contracts.Dtos;
using Mango.Contracts.Models.Service;

namespace Mango.Services.ProductsAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
