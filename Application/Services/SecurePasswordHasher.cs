using krov_nad_glavom_api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace krov_nad_glavom_api.Application.Services
{
    public class SecurePasswordHasher : ISecurePasswordHasher
    {
        private readonly PasswordHasher<object> _hasher;

        public SecurePasswordHasher()
        {
            _hasher = new PasswordHasher<object>();
        }

        public string Hash(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        public bool Verify(string hash, string password)
        {
            var result = _hasher.VerifyHashedPassword(null, hash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}