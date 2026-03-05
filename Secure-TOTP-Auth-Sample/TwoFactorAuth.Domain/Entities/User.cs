namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsTwoFactorEnabled { get; set; }
        public string? TwoFactorSecret { get; set; }
        public string? PendingTwoFactorSecret { get; set; }
        public long? LastUsedTimeStep { get; set; }
        public ICollection<RecoveryCode> RecoveryCodes { get; set; } = new List<RecoveryCode>();
    }
}
