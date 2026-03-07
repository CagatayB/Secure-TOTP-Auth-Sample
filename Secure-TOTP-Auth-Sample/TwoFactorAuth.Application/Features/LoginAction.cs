using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.DTOs;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features
{
    public class LoginAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        // IPasswordHasher interface for hashing and verifying passwords.
        private readonly IPasswordHasher _passwordHasher;

        public async Task<LoginResponse?> Execute(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !_passwordHasher.Verify(user.PasswordHash, password))
                return null; // Invalid credentials

            // Check if 2FA is enabled for the user
            if (user.IsTwoFactorEnabled)
            {
                // Generate a limited-use token for 2FA verification
                var limitedToken = _tokenService.GenerateLimitedToken(user.Id);
                return new LoginResponse(true, limitedToken);
            }

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email);
            return new LoginResponse(false, accessToken);
        }
    }
}
