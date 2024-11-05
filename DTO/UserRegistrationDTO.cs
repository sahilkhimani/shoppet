using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class UserRegistrationDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        public string PhoneNo { get; set; }
        
        [Required]
        public int RoleId { get; set; }
    }
}
