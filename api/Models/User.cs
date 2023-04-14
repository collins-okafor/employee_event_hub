using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class User
    {  
        public int UserId { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Email is a required field!")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(200)]
        [Required(ErrorMessage = "Password is a required field  ")]
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role? Role { get; set; }

    }
} 