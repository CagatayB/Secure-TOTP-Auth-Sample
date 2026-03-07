using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features
{
    public class ConfirmTwoFactorAction
    {
        private readonly ITotpService _totpService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;

        public async Task<bool> Execute(Guid userId, string code)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (string.IsNullOrEmpty(user.PendingTwoFactorSecret))
                return false;

            // 1. Resolve and verify the pending secret.
            var decryptedSecret = _encryptionService.Decrypt(user.PendingTwoFactorSecret);
            var isValid = _totpService.VerifyCode(decryptedSecret, code);

            if (!isValid) return false;

            // 2. Approved! Move the item from pending to production and update its status.
            user.TwoFactorSecret = user.PendingTwoFactorSecret;
            user.PendingTwoFactorSecret = null;
            user.IsTwoFactorEnabled = true;

            // 3. Generate recovery codes
            // user.RecoveryCodes = GenerateRecoveryCodes(); // We will generate recovery codes here.
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
