namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.DTOs
{
    public record ConfirmResult(
     bool Success,
     List<string>? RecoveryCodes = null,
     string? ErrorMessage = null
 );
}
