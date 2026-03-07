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

var app = builder.Build();

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
