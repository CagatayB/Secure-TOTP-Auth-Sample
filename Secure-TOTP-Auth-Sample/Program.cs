using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Features;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Persistence;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Persistence.Repositories;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthorization(options =>
{
    // Only for users who have passed 2FA verification.
    options.AddPolicy("FullAccess", policy =>
        policy.RequireClaim("scope", "full_access"));

    // For users who are only in the 2FA phase.
    options.AddPolicy("TwoFactorOnly", policy =>
        policy.RequireClaim("purpose", "2fa"));
});

// Persistence
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Infrastructure
builder.Services.AddScoped<ITotpService, TotpService>();
builder.Services.AddScoped<IEncryptionService, AesEncryptionService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IRecoveryCodeService, RecoveryCodeService>();

builder.Services.AddScoped<LoginAction>();
builder.Services.AddScoped<SetupTwoFactorAction>();
builder.Services.AddScoped<ConfirmTwoFactorAction>();
builder.Services.AddScoped<ValidateTwoFactorAction>();



// Rate Limiting (Brute Force Attack Protection) ---
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("2fa-policy", opt =>
    {
        opt.PermitLimit = 5; // 5 attempts
        opt.Window = TimeSpan.FromMinutes(2); // In 2 minutes
        opt.QueueLimit = 0;
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter(); // Kimlik doğrulamadan önce veya sonra eklenebilir
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapAuthEndpoints();
app.MapTwoFactorEndpoints();


app.Run();