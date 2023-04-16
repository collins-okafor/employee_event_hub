using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO.AuthDto
{
    public class RegisterDto
    {

        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
    }
}