namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces
{
    public interface IRecoveryCodeService
    {
        // Method to generate a list of recovery codes for 2FA
        List<string> GenerateRawCodes(int count = 10);

        // Method to generate a hash for thee raw recovery code using BCrypt
        string HashCode(string rawCode);

        // Method to verify a raw recovery code against its hashed version
        bool VerifyCode(string rawCode, string hashedCode);
    }
}
