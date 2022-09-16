using Mango.Contracts.Dtos;

namespace Mango.Services.ProductsAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>?> GetProducts();

        Task<ProductDto?> GetProductById(int productId);

        Task<ProductDto> CreateOrUpdateProduct(ProductDto productDto);

        Task<bool> DeleteProduct(int productId);
    }
}
