namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.DTOs
{
    public record LoginResponse(bool RequiresTwoFactor, string Token);
    
}
