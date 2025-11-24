using System.Runtime.CompilerServices;
using App.DTOs;

namespace App.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> CreateUserAsync(UserDTO userDto);
        Task<UserDTO> UpdateUserAsync(string userId, UserDTO userDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> RegisterUserAsync(RegisterDTO registerDTO);
        Task<LoginResponse> LoginAsync(string userName, string password);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task<bool> VerifyPasswordAsync(string userId, string password);
        Task<bool> SendResetPasswordEmailAsync(string email);
        Task LogoutAsync(string userId);
        Task<bool> SendOtpVerifyEmail(string email);
        Task<bool> ConfirmEmailAsync(string email);
    }
}