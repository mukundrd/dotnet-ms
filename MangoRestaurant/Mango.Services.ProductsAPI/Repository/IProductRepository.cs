using Mango.Services.ProductsAPI.DTOs;

namespace Mango.Services.ProductsAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>?> GetProducts();

        Task<ProductDTO?> GetProductById(int productId);

        Task<ProductDTO> CreateOrUpdateProduct(ProductDTO productDTO);

        Task<bool> DeleteProduct(int productId);
    }
}
