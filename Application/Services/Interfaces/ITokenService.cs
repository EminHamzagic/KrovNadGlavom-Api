using System.Security.Claims;

namespace krov_nad_glavom_api.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(string userId, string role);
        ClaimsPrincipal ValidateAccessToken(string token, out bool isExpired);
        string GenerateEmailVerificationToken(string userId, string email);
        string GeneratePasswordResetToken(string userId, string email);
    }
}