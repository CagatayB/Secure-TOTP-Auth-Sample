namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces
{
    public interface ITokenService
    {
        // Method to generate an access token for a user
        string GenerateAccessToken(Guid userId, string email);

        // Method to generate a limited-use token for a specific purpose
        string GenerateLimitedToken(Guid userId, string purpose = "2fa");
    }
}
