using Microsoft.AspNetCore.Identity;
using WalletHub.Services.Interface;

namespace WalletHub.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        private readonly PasswordHasher<string> _passwordHasher = new();

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword("", password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword("", hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
