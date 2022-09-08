using AutoMapper;
using Mango.Services.ProductsAPI.DTOs;
using Mango.Services.ProductsAPI.Models;

namespace Mango.Services.ProductsAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
