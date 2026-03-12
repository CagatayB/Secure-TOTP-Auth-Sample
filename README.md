# 🔐 Secure TOTP & 2FA Implementation (.NET)
This repository demonstrates a production-ready Two-Factor Authentication (2FA) system built with .NET 8/9, following Clean Architecture principles and security best practices.

It implements Time-based One-Time Passwords (TOTP) using standard RFC 6238, featuring a secure dual-token authentication flow.

# ✨ Key Features
* Clean Architecture: Decoupled layers (Domain, Application, Infrastructure, Persistence, API) for high maintainability.

* TOTP Integration: Support for Google Authenticator, Microsoft Authenticator, and Authy.

* Dual-Token Strategy: Initial login provides a Limited Access Token (2FA-only), which is upgraded to a Full Access Token upon successful TOTP verification.

* Secrets at Rest: All TOTP shared secrets are stored using AES-256 encryption in the database.

* Recovery Codes: Automated generation of one-time recovery codes, stored using BCrypt hashing.

* Brute-Force Protection: Integrated Fixed Window Rate Limiting on sensitive 2FA endpoints.

* Replay Attack Prevention: Time-step tracking to ensure each TOTP code is used only once.

# 🏗️ Architecture Overview
*Domain: Core entities (User, RecoveryCode).

* Application: Business logic, interfaces, and Use Cases (SetupTwoFactor, ValidateTwoFactor).

* Infrastructure: External concerns like TOTP generation, AES encryption, and JWT handling.

* Persistence: EF Core implementation and Repository patterns.

* API: Minimal API endpoints and security middleware.

# 🚀 Tech Stack
* Framework: ASP.NET Core 8+ (Minimal APIs)

* Database: MS SQL Server / EF Core

* Security: Otp.NET, QRCoder, BCrypt.Net, JWT Bearer

* Containerization: Docker Ready (optional)

# 🛠️ Getting Started
1. Prerequisites
* .NET 8 SDK or higher
* SQL Server (LocalDB or Docker)

2. Configuration
Update appsettings.json with your secrets:
```
JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=TwoFactorAuthDb;..."
  },
  "JwtSettings": { "Key": "YOUR_SECRET_KEY", "Issuer": "TwoFactorAuthApi" },
  "EncryptionSettings": { "Key": "32_CHARACTER_AES_KEY", "IV": "16_CHAR_IV" }
}
```
3. Database Setup

```
dotnet ef database update --project TwoFactorAuth.Persistence --startup-project TwoFactorAuth.Api
```
4. Run `dotnet restore`
5. Run `dotnet run`

# 🔄 Authentication Flow
1. Register: Create an account via /auth/register.
2. Login (Step 1): Login via /auth/login. If 2FA is enabled, you receive a Limited Token.
3. Setup 2FA: Call /2fa/setup with your token to get a QR Code.
4. Confirm 2FA: Verify the first code via /2fa/confirm. This activates 2FA and generates Recovery Codes.
5. Final Validation (Step 2): On subsequent logins, verify the TOTP code via /2fa/validate to receive your Full Access Token.

# 🛡️ Security Considerations
Rate Limiting (5 attempts per 2 minutes) on the /2fa/validate endpoint to prevent brute-forcing the 6-digit TOTP codes. All secrets are encrypted before hitting the disk to ensure that even a database leak does not compromise user security.
