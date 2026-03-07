namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.DTOs
{
    public record SetupTwoFactorResponse(string ManualEntryKey, string QrCodeImageUrl);
}
