using Mango.Contracts.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new List<ProductDto>();
            var response = await _productService.GetAllProductsAsync<ResponseDto>(await GetAccessToken());
            if (response != null)
            {
                list = Deserialize<List<ProductDto>>(response.Result);
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        public async Task<IActionResult> Create(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDto>(await GetAccessToken(), product);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ProductDto? product;
            if (ModelState.IsValid)
            {
                var response = await _productService.GetProductByIdAsync<ResponseDto>(await GetAccessToken(), productId);
                if (response != null && response.IsSuccess)
                {
                    product = Deserialize<ProductDto>(response.Result);
                    return View(product);
                }
            }
            return RedirectToAction(nameof(ProductIndex));
        }

        public async Task<IActionResult> Edit(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProductAsync<ResponseDto>(await GetAccessToken(), product);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(product);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.DeleteProductsAsync<ResponseDto>(await GetAccessToken(), productId);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return RedirectToAction(nameof(ProductIndex));
        }

    }
}
