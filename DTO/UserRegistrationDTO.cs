﻿using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class UserRegistrationDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "valid email address is required")]
        public string UserEmail { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!#.^%*?&])[A-Za-z\d@$!#.^%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, and one number and one special character.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Select the Role")]
        public string RoleId { get; set; }
    }
}
