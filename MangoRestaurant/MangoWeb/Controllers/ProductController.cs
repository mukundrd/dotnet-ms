using Mango.Services.ProductsAPI.DTOs;
using MangoWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace MangoWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? list = new List<ProductDTO>();
            var response = await _productService.GetAllProductsAsync<ResponseDTO>();
            if (response != null)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        public async Task<IActionResult> Create(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDTO>(product);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ProductDTO? product;
            if (ModelState.IsValid)
            {
                var response = await _productService.GetProductByIdAsync<ResponseDTO>(productId);
                if (response != null && response.IsSuccess)
                {
                    product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                    return View(product);
                }
            }
            return RedirectToAction(nameof(ProductIndex));
        }

        public async Task<IActionResult> Edit(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProductAsync<ResponseDTO>(product);
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
                var response = await _productService.DeleteProductsAsync<ResponseDTO>(productId);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return RedirectToAction(nameof(ProductIndex));
        }

    }
}
