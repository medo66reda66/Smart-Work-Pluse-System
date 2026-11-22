using Microsoft.IdentityModel.Tokens;
using SmartWorkSpasePLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class TokenService : ITokenService
    {
        public string CreateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Nf7$Pq19!sD@84LmZ#xT2wQvR%k9Hp36"));
            var securityKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7295",
                audience: "https://localhost:7295",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: securityKey
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
