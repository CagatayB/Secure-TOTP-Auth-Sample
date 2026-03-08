namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces
{
    public interface IPasswordHasher
    {
        // Method to hash a password
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}
