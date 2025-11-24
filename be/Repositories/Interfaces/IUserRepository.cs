


using App.Data;

namespace App.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
        Task UpdateUserAsync( ApplicationUser user);
        Task<bool> DeleteUserAsync(ApplicationUser user);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsEmailExistsForUpdateAsync(string email, string userId);
        Task<string> GetUserRoleAsync(ApplicationUser user);
        
        Task<ApplicationUser> GetUserByUsernameAsync(string userName);
        Task<ApplicationUser?> GetUserByRefreshTokenAsync(string refreshToken);
        Task UpdateRefreshTokenAsync(string userId, string refreshToken, DateTime expiry);
    }
}