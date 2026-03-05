namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
