using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopApi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        
        [Required]
        public byte[] Password { get; set; }
        public string PhoneNo { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Pet> Pets { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
