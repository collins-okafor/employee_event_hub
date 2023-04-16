using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.JWT
{
    public class TokenManager : ITokenManager
    {
        private readonly SymmetricSecurityKey _key;

        public TokenManager(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["HubJWT:Secret"] ?? ""));
        }
        
        public string GenerateToken(User user, string roleName)
        {
            var claims = new List<Claim>{
               new Claim(JwtRegisteredClaimNames.Name,user.Email),
               new Claim(ClaimTypes.Role,roleName)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = credentials,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}