using Microsoft.AspNetCore.RateLimiting;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Persistence
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Infrastructure
builder.Services.AddScoped<ITotpService, TotpService>();
builder.Services.AddScoped<IEncryptionService, AesEncryptionService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IRecoveryCodeService, RecoveryCodeService>();

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
app.UseRateLimiter();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
