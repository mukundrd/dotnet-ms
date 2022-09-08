using Mango.Services.ProductsAPI.DTOs;
using Mango.Services.ProductsAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductsAPI.Controllers
{
    [Route("api/products")]
    public class ProductsAPIController : Controller
    {

        protected ResponseDTO _response;

        private IProductRepository _productRepository;

        public ProductsAPIController(IProductRepository productRepository)
        {
            _response = new ResponseDTO();
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ResponseDTO> Get()
        {
            try
            {
                IEnumerable<ProductDTO> productDTOs = await _productRepository.GetProducts();
                _response.Result = productDTOs;
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
        public async Task<ResponseDTO> Get(int id)
        {
            try
            {
                ProductDTO? productDTO = await _productRepository.GetProductById(id);
                _response.Result = productDTO;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Post([FromBody] ProductDTO product)
        {
            try
            {
                ProductDTO productDTO = await _productRepository.CreateOrUpdateProduct(product);
                _response.Result = productDTO;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        [HttpPut]
        public async Task<ResponseDTO> Put([FromBody] ProductDTO product)
        {
            try
            {
                ProductDTO productDTO = await _productRepository.CreateOrUpdateProduct(product);
                _response.Result = productDTO;
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
        public async Task<ResponseDTO> Delete(int id)
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
