using Microsoft.Identity.Client;

namespace shoppetApi.Interfaces
{
    public interface IHttpContextHelper
    {
        public string GetCurrentUserId();
        public string GetCurrentUserRole();
    }
}
