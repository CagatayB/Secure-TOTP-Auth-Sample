using OtpNet;
using QRCoder;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services
{
    public class TotpService : ITotpService
    {
        private const string Issuer = "MySecureApp"; // Your application name (shown in the authenticator app)

        public (string Secret, string QrCodeUri) GenerateSetupData(string email)
        {
            // 1. Generate a secret key for a user (20 bytes)
            // The Base32-encoded shared secretThis key should be stored securely in the database associated with the user.
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            string base32Secret = Base32Encoding.ToString(secretKey);

            // 2. Generating otpauth URLs for authenticator applications.    
            // Format: otpauth://totp/Issuer:UserEmail?secret=SECRET&issuer=Issuer&digits=6&period=30
            // period - How often the code rotates in seconds (standard is 30) 
            string qrCodeUri = $"otpauth://totp/{Uri.EscapeDataString(Issuer)}:{Uri.EscapeDataString(email)}" +
                               $"?secret={base32Secret}&issuer={Uri.EscapeDataString(Issuer)}&digits=6&period=30";

            return (base32Secret, qrCodeUri);
        }

        public bool VerifyCode(string secret, string code)
        {
            byte[] secretKey = Base32Encoding.ToBytes(secret);
            var totp = new Totp(secretKey);

            // To prevent clock drift between the user and the server.
            // It allows a tolerance of 30 seconds.
            return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }

        // Generate the QR code
        public byte[] GenerateQrCodeImage(string qrCodeUri)
        {
            using var qrGenerator = new QRCodeGenerator();

            // ECCLevel.Q gives us a good balance between error correction and data capacity. It can recover up to 25% of the data if the QR code is damaged.
            using var qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(10);
        }
    }
}
