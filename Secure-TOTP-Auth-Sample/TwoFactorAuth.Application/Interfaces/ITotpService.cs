namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces
{
    public interface ITotpService
    {
        (string Secret, string QrCodeUri) GenerateSetupData(string email);
        bool VerifyCode(string secret, string code);
        byte[] GenerateQrCodeImage(string qrCodeUri);
    }
}
