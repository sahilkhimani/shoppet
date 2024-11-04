using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopApi.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        
        [Required]
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
