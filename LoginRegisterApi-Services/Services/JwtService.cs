using LoginRegisterApi.Models;
using LoginRegisterApi_Services.Services.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginRegisterApi_Services.Services
{
    public class JwtService : IJwtService
    {

        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GenerateJwtToken(UserDetail user)

        {
            var claim = new List<Claim>
        {
            new Claim("UserName", user.Email),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claim,
                expires: expires,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null)
            {
                return false;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;
                if (jwtSecurityToken != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetTokenData(string? token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:key"]);

            if (key == null) { return ""; }
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var RoleID = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            return RoleID;
        }
    }
}

