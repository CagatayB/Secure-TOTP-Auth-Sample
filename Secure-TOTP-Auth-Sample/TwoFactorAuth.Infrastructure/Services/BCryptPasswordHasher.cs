using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using System.Drawing.Text;
using BC = BCrypt.Net.BCrypt;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        //  WorkFactor parameter that controls the computational difficulty and thus the time required to hash a password.
        private const int WorkFactor = 12;
        public string Hash(string password)
        {
            // BCrypt automatically generates a random salt for every hashing.
            return BC.HashPassword(password, WorkFactor);
        }

        public bool Verify(string password, string hashedPassword)
        {
            // Compares the password with the stored hash in the database.
            return BC.Verify(password, hashedPassword);
        }
    }
}
