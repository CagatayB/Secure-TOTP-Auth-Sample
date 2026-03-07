using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features
{
    public class ValidateTwoFactorAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ITotpService _totpService;
        private readonly IEncryptionService _encryptionService;

        public async Task<string?> Execute(Guid userId, string code) 
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null || !user.IsTwoFactorEnabled || string.IsNullOrEmpty(user.TwoFactorSecret))
                return null; // User not found or 2FA not enabled

            var secret = _encryptionService.Decrypt(user.TwoFactorSecret);

            // TOTP verification (with Replay Attack protection)
            // The VerifyCode method must validate the last used TimeStep"
            bool isValid = _totpService.VerifyCode(secret, code);

            if (!isValid) return null; // Invalid 2FA code

            // Generate an access token upon successful 2FA validation
            return _tokenService.GenerateAccessToken(user.Id, user.Email);
        }

    }
}
