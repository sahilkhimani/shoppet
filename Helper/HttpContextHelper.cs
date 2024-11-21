using PetShopApi.Models;
using shoppetApi.Interfaces;
using System.Security.Claims;

namespace shoppetApi.Helper
{
    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserId()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            return httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
