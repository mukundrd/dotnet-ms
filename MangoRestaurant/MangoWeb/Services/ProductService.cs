using Mango.Services.ProductsAPI.DTOs;

namespace MangoWeb.Services
{
    public class ProductService : BaseService, IProductService
    {

        private readonly IHttpClientFactory clientFactory;

        public ProductService(IHttpClientFactory clientFactory): base(clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<T?> CreateProductAsync<T>(ProductDTO product)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = product,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = ""
            });
        }

        public async Task<T?> DeleteProductsAsync<T>(int id)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = ""
            });
        }

        public async Task<T?> GetAllProductsAsync<T>()
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = ""
            });
        }

        public async Task<T?> GetProductByIdAsync<T>(int id)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = ""
            });
        }

        public async Task<T?> UpdateProductAsync<T>(ProductDTO product)
        {
            return await SendAsync<T>(new Models.APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = product,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = ""
            });
        }
    }
}
