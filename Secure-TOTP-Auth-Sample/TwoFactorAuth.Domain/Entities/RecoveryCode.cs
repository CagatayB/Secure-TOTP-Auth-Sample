namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Domain.Entities
{
    public class RecoveryCode
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CodeHash { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
