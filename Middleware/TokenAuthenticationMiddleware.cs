using krov_nad_glavom_api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace krov_nad_glavom_api.Middleware
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;

        public TokenAuthenticationMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            var sessionService = context.RequestServices.GetRequiredService<IUserSessionService>();

            string accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string refreshToken = context.Request.Headers["X-Refresh-Token"].FirstOrDefault();

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access and refresh tokens are required.");
                return;
            }

            var principal = _tokenService.ValidateAccessToken(accessToken, out bool isExpired);

            if (principal == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid access token.");
                return;
            }

            if (principal != null && !isExpired)
            {
                context.User = principal;
                await _next(context);
                return;
            }

            if (isExpired)
            {
                var storedSession = await sessionService.GetSessionByRefreshToken(refreshToken);

                if (storedSession == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Refresh token not found or already invalidated.");
                    return;
                }

                if (storedSession.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    await sessionService.DeleteSession(refreshToken);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Refresh token expired. Please log in again.");
                    return;
                }

                // Issue new JWT and extend refresh token
                var newAccessToken = _tokenService.GenerateAccessToken(storedSession.UserId, storedSession.Role);
                var newExpiry = DateTime.UtcNow.AddMinutes(15);
                await sessionService.UpdateRefreshTokenExpiry(refreshToken, newExpiry);

                // Add new JWT to response headers
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers["X-New-Access-Token"] = newAccessToken;
                    context.Response.Headers["X-Refresh-Token-Expiry"] = newExpiry.ToString("o");
                    return Task.CompletedTask;
                });

                // Re-validate with new access token
                var newPrincipal = _tokenService.ValidateAccessToken(newAccessToken, out _);
                context.User = newPrincipal;

                await _next(context);
            }
        }
    }
}