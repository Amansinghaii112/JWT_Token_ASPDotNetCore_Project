using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace JWT_Token_Project
{
    public class JWTTokenHelper : IJWTTokenHelper
    {
        private readonly JWTSeurityTokenOption _jwtSeurityToken;
        private readonly byte[] _securityTokenKey;
        public JWTTokenHelper(IOptions<JWTSeurityTokenOption> jwtSeurityToken)
        {
            _jwtSeurityToken = jwtSeurityToken.Value;
            _securityTokenKey = Encoding.ASCII.GetBytes(_jwtSeurityToken.TokenKey);
        }

        public string JWTTokenGenerator(UserDTO user)
        {
            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email)
                }
                ),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_securityTokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new();
            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}
