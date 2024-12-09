using Microsoft.AspNetCore.Identity;

namespace PetShopApi.Models
{
    public class User : IdentityUser

    {
        public ICollection<Pet> Pets { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
