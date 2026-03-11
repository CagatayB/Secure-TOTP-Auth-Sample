using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using System.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services
{
    public class RecoveryCodeService : IRecoveryCodeService
    {
        public List<string> GenerateRawCodes(int count = 10)
        {
            var codes = new List<string>();
            for (int i = 0; i < count; i++)
            {
                // Generate a cryptographically random code
                var bytes = RandomNumberGenerator.GetBytes(5);
                codes.Add(Convert.ToHexString(bytes).ToLower());
            }
            return codes;
        }

        public string HashCode(string rawCode){

            // Hash the code using BCrypt with a strong work factor
            return BC.HashPassword(rawCode);
        }

        public bool VerifyCode(string rawCode, string hashedCode)
        {
            return BC.Verify(rawCode, hashedCode);
        }
    }
}
