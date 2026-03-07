using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Domain.Entities;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.DTOs;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features
{
    public class ConfirmTwoFactorAction
    {
        private readonly ITotpService _totpService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;
        private readonly IRecoveryCodeService _recoveryCodeService;

        public async Task<ConfirmResult> Execute(Guid userId, string code)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (string.IsNullOrEmpty(user.PendingTwoFactorSecret))
                return new ConfirmResult(false);

            // 1. Resolve and verify the pending secret.
            var decryptedSecret = _encryptionService.Decrypt(user.PendingTwoFactorSecret);
            var isValid = _totpService.VerifyCode(decryptedSecret, code);

            if (!isValid) return new ConfirmResult(false); ;

            // 2. Approved! Move the item from pending to production and update its status.
            user.TwoFactorSecret = user.PendingTwoFactorSecret;
            user.PendingTwoFactorSecret = null;
            user.IsTwoFactorEnabled = true;


            // 3. Generate recovery codes
            var rawCodes = _recoveryCodeService.GenerateRawCodes();

            user.RecoveryCodes = rawCodes.Select(code => new RecoveryCode
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CodeHash = _recoveryCodeService.HashCode(code),
                IsUsed = false
            }).ToList();

            await _userRepository.UpdateAsync(user);

            // Return plain text codes to show the user ONCE
            return new ConfirmResult(true, rawCodes);
        }
    }
}
