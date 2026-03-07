using Microsoft.AspNetCore.DataProtection;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.DTOs;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features
{
    public class SetupTwoFactorAction
    {
        private readonly ITotpService _totpService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;

        public async Task<SetupTwoFactorResponse> Execute(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            // 1. Generate a new Secret and QR URI.
            var (secret, qrUri) = _totpService.GenerateSetupData(user.Email);

            // 2. Encrypt the Secret and save it as "Pending".
            user.PendingTwoFactorSecret = _encryptionService.Encrypt(secret);
            await _userRepository.UpdateAsync(user);

            // 3. Convert to QR code image (we can return it as Base64)
            var qrCodeBytes = _totpService.GenerateQrCodeImage(qrUri);
            var qrCodeBase64 = $"data:image/png;base64,{Convert.ToBase64String(qrCodeBytes)}";

            return new SetupTwoFactorResponse(secret, qrCodeBase64);
        }
    }
}
