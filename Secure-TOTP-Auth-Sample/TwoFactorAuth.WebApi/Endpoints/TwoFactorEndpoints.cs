using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features;
using Microsoft.AspNetCore.Mvc;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.WebApi.Endpoints
{
    public static class TwoFactorEndpoints
    {
        public static void MapTwoFactorEndpoints(this IEndpointRouteBuilder app)
        {
            // All 2FA endpoints require authorization (Limited or Full Token)
            var group = app.MapGroup("/2fa").RequireAuthorization();

            // 1. Start and generate a QR code.
            group.MapPost("/setup", async (Guid userId, SetupTwoFactorAction action) =>
            {
                var result = await action.Execute(userId);
                return Results.Ok(result);
            });

            // 2. Activate 2FA by entering the first code.
            group.MapPost("/confirm", async (Guid userId, string code, ConfirmTwoFactorAction action) =>
            {
                var result = await action.Execute(userId, code);
                return result.Success ? Results.Ok(result) : Results.BadRequest("Invalid code.");
            });

            // 3. Verify your 2FA code after logging in (get a Full Access Token).
            group.MapPost("/validate", async (Guid userId, string code, ValidateTwoFactorAction action) =>
            {
                var fullAccessToken = await action.Execute(userId, code);
                return fullAccessToken != null ? Results.Ok(new { Token = fullAccessToken }) : Results.Unauthorized();
            }).RequireRateLimiting("2fa-validation");

        }
    }
}
