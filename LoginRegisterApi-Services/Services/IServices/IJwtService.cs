using LoginRegisterApi.Models;
using System.IdentityModel.Tokens.Jwt;


namespace LoginRegisterApi_Services.Services.IServices
{
    public interface IJwtService
    {
        public string GenerateJwtToken(UserDetail user);

        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);

        public string GetTokenData(string? token);
    }
}
