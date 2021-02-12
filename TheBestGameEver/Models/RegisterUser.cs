using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TheBestGameEver.Models
{
    public class RegisterUser
    {
            
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        //optional
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}

