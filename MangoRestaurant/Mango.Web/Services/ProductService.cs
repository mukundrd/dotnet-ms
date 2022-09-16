using Mango.Contracts.Dtos;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {

        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory) { }

        public async Task<T?> CreateProductAsync<T>(string token, ProductDto product)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = product,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T?> DeleteProductsAsync<T>(string token, int id)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = token
            });
        }

        public async Task<T?> GetAllProductsAsync<T>(string token)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T?> GetProductByIdAsync<T>(string token, int id)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = token
            });
        }

        public async Task<T?> UpdateProductAsync<T>(string token, ProductDto product)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = product,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }
    }
}
