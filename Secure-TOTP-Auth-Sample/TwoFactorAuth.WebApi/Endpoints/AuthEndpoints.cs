using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.WebApi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/auth");

            group.MapPost("/login", async (
            [FromBody] LoginRequest request,
            [FromServices] LoginAction loginAction) =>
            {
                var result = await loginAction.Execute(request.Email, request.Password);

                if (result == null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(result);
            });
        }
    }
    public record LoginRequest(string Email, string Password);
}
