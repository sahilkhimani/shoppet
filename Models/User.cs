using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopApi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required(ErrorMessage ="Please enter the username")]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage ="valid email address is required")]
        public string UserEmail { get; set; }
        
        [Required]
        public byte[] Password { get; set; }
        public string PhoneNo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Please Select the role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Pet> Pets { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
