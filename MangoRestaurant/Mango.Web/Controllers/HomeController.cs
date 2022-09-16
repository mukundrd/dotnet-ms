using Mango.Contracts.Dtos;
using Mango.Contracts.Models.Client;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductDto> products;
        var response = await _productService.GetAllProductsAsync<ResponseDto>("");
        if (response != null && response.IsSuccess)
        {
            products = Deserialize<List<ProductDto>>(response.Result);
        }
        else
        {
            products = new();
        }
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int productId)
    {
        ProductDto product;
        var response = await _productService.GetProductByIdAsync<ResponseDto>("", productId);
        if (response != null && response.IsSuccess)
        {
            product = Deserialize<ProductDto>(response.Result);
        }
        else
        {
            product = new();
        }
        return View(product);
    }

    [HttpPost]
    [ActionName("Details")]
    [Authorize]
    public async Task<object> DetailsPost(ProductDto productDto)
    {
        CartDto cartDto = new CartDto
        {
            CartHeader = new CartHeaderDto
            {
                UserId = GetUserid()
            }
        };
        CartDetailDto cartDetailDto = new CartDetailDto
        {
            Count = productDto.count,
            ProductId = productDto.ProductId
        };
        var resp = await _productService.GetProductByIdAsync<ResponseDto>("", productDto.ProductId);

        if (resp?.IsSuccess == true)
        {
            cartDetailDto.Product = Deserialize<ProductDto>(resp.Result);
        }
        List<CartDetailDto> cartDetailDtos = new List<CartDetailDto> { cartDetailDto };
        cartDto.CartDetails = cartDetailDtos;

        var addToCartResponse = await _cartService.AddToCartAsync<ResponseDto>(await GetAccessToken(), cartDto);
        if (addToCartResponse?.IsSuccess == true)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(productDto);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        Console.WriteLine("Token : " + await GetAccessToken());
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

}

