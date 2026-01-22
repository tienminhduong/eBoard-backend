using eBoardAPI.Consts;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Services;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace eBoardAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(Teacher teacher)
        {
            var claims = new[]
            {
            new Claim(CustomClaims.USER_ID, teacher.Id.ToString()),
            new Claim(ClaimTypes.Role, nameof(ROLE.Teacher))
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(EnvKey.JWT_KEY)!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable(EnvKey.JWT_ISSUER)!,
                audience: Environment.GetEnvironmentVariable(EnvKey.JWT_AUDIENCE)!,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateAccessToken(Parent parent)
        {
            // Implement similarly to GenerateAccessToken for Teacher
            var claims = new[]
            {
                new Claim(CustomClaims.USER_ID, parent.Id.ToString()),
                new Claim(ClaimTypes.Role, ROLE.Parent.ToString())
                };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(EnvKey.JWT_KEY)!)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable(EnvKey.JWT_ISSUER)!,
                audience: Environment.GetEnvironmentVariable(EnvKey.JWT_AUDIENCE)!,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string GenerateResetPasswordToken(Teacher teacher)
        {
            var claims = new[]
            {
                new Claim(CustomClaims.USER_ID, teacher.Id.ToString()),
                new Claim(ClaimTypes.Role, ROLE.Teacher.ToString()),
                new Claim(CustomClaims.TYPE, "reset-password")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(EnvKey.JWT_KEY)!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable(EnvKey.JWT_ISSUER)!,
                audience: Environment.GetEnvironmentVariable(EnvKey.JWT_AUDIENCE)!,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
