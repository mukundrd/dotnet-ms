using Mango.Contracts.Dtos;
using Mango.Services.ProductsAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductsAPI.Controllers
{
    [Route("api/products")]
    public class ProductsAPIController : Controller
    {

        protected ResponseDto _response;

        private IProductRepository _productRepository;

        public ProductsAPIController(IProductRepository productRepository)
        {
            _response = new ResponseDto();
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await _productRepository.GetProducts();
                _response.Result = productDtos;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDto> Get(int id)
        {
            try
            {
                ProductDto? ProductDto = await _productRepository.GetProductById(id);
                _response.Result = ProductDto;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> Post([FromBody] ProductDto product)
        {
            try
            {
                ProductDto ProductDto = await _productRepository.CreateOrUpdateProduct(product);
                _response.Result = ProductDto;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseDto> Put([FromBody] ProductDto product)
        {
            try
            {
                ProductDto ProductDto = await _productRepository.CreateOrUpdateProduct(product);
                _response.Result = ProductDto;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                bool IsDelete = await _productRepository.DeleteProduct(id);
                _response.Result = IsDelete;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }
    }
}
