using App.DTOs;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int? page, int? limit)
        {
            var users = await _userService.GetAllUsersAsync(page, limit);
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách users thành công ",
                data = users.Data,
                pagination = new
                {
                    total = users.Total,
                    totalPages = page.HasValue ? users.TotalPages : null,
                    currentPage = page.HasValue ? users.CurrentPage : null,
                    limit = page.HasValue ? users.Limit : null
                }
            }
            );
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.DeleteUserAsync(id);

            if (!user)
                return NotFound(new
                {
                    success = false,
                    message = "Xóa thất bại"
                });

            return Ok(new
            {
                success = true,
                message = "Xóa user thành công ",
                data = user
            }
              );
        }

        [HttpPut("/lock/{id}")]
        public async Task<IActionResult> LockUser(string id)
        {
            var user = await _userService.LockUserAsync(id);

            if (!user)
                return NotFound(new
                {
                    success = false,
                    message = "khóa thất bại"
                });

            return Ok(new
            {
                success = true,
                message = "Khóa user thành công ",
                data = user
            }
              );
        }

        [HttpPut("/unlock/{id}")]
        public async Task<IActionResult> UnLockUser(string id)
        {
            var user = await _userService.UnLockUserAsync(id);

            if (!user)
                return NotFound(new
                {
                    success = false,
                    message = "Mở khóa thất bại"
                });

            return Ok(new
            {
                success = true,
                message = " Mở Khóa user thành công ",
                data = user
            }
              );
        }
    }
}
