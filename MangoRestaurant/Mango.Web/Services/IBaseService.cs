using Mango.Services.ProductsAPI.DTOs;
using MangoWeb.Models;

namespace MangoWeb.Services
{
    public interface IBaseService : IDisposable
    {
        ResponseDTO responseModel { get; set; }

        Task<T?> SendAsync<T>(APIRequest apiRequest);
    }
}
