using App.DTOs;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(new
            {
                success = true,
                data = users
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Không tìm thấy user với id {id}"
                });
            }

            return Ok(new
            {
                success = true,
                data = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO dto)
        {
            var user = await _userService.CreateUserAsync(dto);

            return Ok(new
            {
                success = true,
                data = user,
                message = "Tạo user thành công"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDTO dto)
        {
            var user = await _userService.UpdateUserAsync(id, dto);

            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Không tìm thấy user với id {id}"
                });
            }

            return Ok(new
            {
                success = true,
                data = user,
                message = "Cập nhật user thành công"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);

            return Ok(new
            {
                success = true,
                message = "Xóa user thành công"
            });
        }
    }
}
