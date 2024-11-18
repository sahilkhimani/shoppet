using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetShopApi.Models
{
    public class Role : IdentityRole
    {
        public string RoleDescription { get; set; }
    }
}
