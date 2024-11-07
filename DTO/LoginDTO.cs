﻿using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage ="Please enter the valid email address")]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
