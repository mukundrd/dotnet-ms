using Mango.Services.ProductsAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MangoWeb.Services
{
    public interface IProductService: IBaseService
    {
        Task<T?> GetAllProductsAsync<T>();

        Task<T?> GetProductByIdAsync<T>(int id);

        Task<T?> CreateProductAsync<T>(ProductDTO product);

        Task<T?> UpdateProductAsync<T>(ProductDTO product);

        Task<T?> DeleteProductsAsync<T>(int id);
    }
}
