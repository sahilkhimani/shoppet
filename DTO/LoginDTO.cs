using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
