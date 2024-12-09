using Microsoft.AspNetCore.Identity;

namespace PetShopApi.Models
{
    public class Role : IdentityRole
    {
        public string RoleDescription { get; set; }
    }
}
