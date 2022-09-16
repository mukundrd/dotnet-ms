using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<string?> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }

        protected string? GetUserid()
        {
            return User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
        }

        protected T? Deserialize<T>(object data)
        {
            return JsonConvert.DeserializeObject<T>(Convert.ToString(data));
        }
    }
}
