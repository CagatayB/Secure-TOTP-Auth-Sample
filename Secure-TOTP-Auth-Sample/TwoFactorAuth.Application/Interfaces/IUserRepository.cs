using Secure_TOTP_Auth_Sample.TwoFactorAuth.Domain.Entities;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task AddAsync(User user); // User registration
        Task SaveChangesAsync();
    }
}
