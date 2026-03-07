using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        // A strong 32-char key (must come from Appsettings)
        // The key information should be stored in locations such as Azure Key Vault or AWS KMS.
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("_THIS_IS_A_VERY_SECRET_KEY_32_CHAR_!");
        private static readonly byte[] Iv = Encoding.UTF8.GetBytes("_IV_16_CHAR_VAL_");

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = Iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = Iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
