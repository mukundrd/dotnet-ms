using Mango.Contracts.Dtos;

namespace Mango.Web.Services
{
    public interface IProductService: IBaseService
    {
        Task<T?> GetAllProductsAsync<T>(string token);

        Task<T?> GetProductByIdAsync<T>(string token, int id);

        Task<T?> CreateProductAsync<T>(string token, ProductDto productDto);

        Task<T?> UpdateProductAsync<T>(string token, ProductDto productDto);

        Task<T?> DeleteProductsAsync<T>(string token, int id);
    }
}
