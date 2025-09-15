using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Data.Config;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace krov_nad_glavom_api.Application.Services
{
    public class TokenService : ITokenService
    {
		private readonly JWTSettings _jWTSettings;
		private readonly byte[] _key;

        public TokenService(JWTSettings jWTSettings)
        {
            _jWTSettings = jWTSettings;
            _key = Encoding.UTF8.GetBytes(_jWTSettings.Secret);
        }

        public string GenerateAccessToken(string userId, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateAccessToken(string token, out bool isExpired)
        {
            isExpired = false;

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jWTSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jWTSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_key),
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                isExpired = true;
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}