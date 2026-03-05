using OtpNet;
using QRCoder;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services
{
    public class TotpService : ITotpService
    {
        private const string Issuer = "MySecureApp"; // Uygulama adınız

        public (string Secret, string QrCodeUri) GenerateSetupData(string email)
        {
            // 1. Cryptographically secure random key üretimi (20 bytes)
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            string base32Secret = Base32Encoding.ToString(secretKey);

            // 2. Authenticator uygulamaları için otpauth URI oluşturma
            // Format: otpauth://totp/Issuer:UserEmail?secret=SECRET&issuer=Issuer&digits=6&period=30
            string qrCodeUri = $"otpauth://totp/{Uri.EscapeDataString(Issuer)}:{Uri.EscapeDataString(email)}" +
                               $"?secret={base32Secret}&issuer={Uri.EscapeDataString(Issuer)}&digits=6&period=30";

            return (base32Secret, qrCodeUri);
        }

        public bool VerifyCode(string secret, string code)
        {
            byte[] secretKey = Base32Encoding.ToBytes(secret);
            var totp = new Totp(secretKey);
         
            // Kullanıcı ile sunucu arasındaki zaman kaymasını (clock drift) önlemek için 
            // ±30 saniyelik bir tolerans tanır.
            return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }

        public byte[] GenerateQrCodeImage(string qrCodeUri)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(10);
        }
    }
}
