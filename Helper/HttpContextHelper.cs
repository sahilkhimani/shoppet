using Microsoft.Identity.Client;
using PetShopApi.Models;
using shoppetApi.Interfaces;
using System.Security.Claims;

namespace shoppetApi.Helper
{
    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext httpContext;
        public HttpContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            httpContext = _httpContextAccessor.HttpContext!;
        }
        public string GetCurrentUserId()
        {
            return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
        public string GetCurrentUserRole()
        {
            var UserRole = httpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .FirstOrDefault();
            return UserRole!;
        }
    }
}
