using App.DTOs;
using App.Services;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly OtpService _otpService;
        public AuthController(IUserService userService, OtpService otpService)
        {
            _userService = userService;
            _otpService = otpService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            var response = await _userService.LoginAsync(loginDTO.UserName, loginDTO.Password);
            if (response?.AccessToken == null)
                return Unauthorized("Sai thông tin đăng nhập.");

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            var result = await _userService.RegisterUserAsync(registerDTO);
            if (result == false)
                return BadRequest("Đăng ký thất bại. Vui lòng thử lại.");
            return Ok(new { message = "Đăng ký thành công, vui lòng kiểm tra email để xác minh OTP." });
        }

        [HttpPost("verify-password")]
        public async Task<IActionResult> VerifyPassword([FromBody] VerifyPasswordDTO dto)
        {
            var isValid = await _userService.VerifyPasswordAsync(dto.UserId, dto.Password);
            if (!isValid)
                return BadRequest("Mật khẩu không đúng.");
            return Ok("Mật khẩu hợp lệ.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            var isSend = await _userService.SendOtpVerifyEmail(forgotPasswordDTO.Email);
            if (!isSend)
                return BadRequest(new { message = "Không thể gửi mã OTP. Vui lòng thử lại sau." });

            return Ok(new { message = "Mã OTP đã được gửi đến email của bạn." });
        }

        // Xác thực OTP
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpDTO dto)
        {
             var isValid = _otpService.ValidateOtp(dto.Email, dto.Otp);
            if (!isValid)
                return BadRequest(new { message = "OTP không đúng hoặc đã hết hạn." });

            // Xác nhận email
            var result = await _userService.ConfirmEmailAsync(dto.Email);
            if (!result)
                return BadRequest(new { message = "Không thể xác nhận email. Vui lòng thử lại." });

            return Ok(new { message = "Xác nhận email thành công. Bạn có thể đăng nhập ngay bây giờ." });
        }

        [HttpPost("verify-otp-reset-password")]
        public IActionResult VerifyOtpResetPassword([FromBody] VerifyOtpDTO dto)
        {
            var isValid = _otpService.ValidateOtp(dto.Email, dto.Otp);
            if (!isValid)
                return BadRequest(new { message = "OTP không đúng hoặc đã hết hạn." });
                
            return Ok(new { message = "Xác thực OTP thành công. Bạn có thể đặt lại mật khẩu." });
        }
        
        // Quên mật khẩu
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var success = await _userService.ResetPasswordAsync(dto.Email, dto.NewPassword);
            if (!success)
                return BadRequest("Không thể đặt lại mật khẩu.");
            return Ok("Đặt lại mật khẩu thành công.");
        }

        // Đổi mật khẩu
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var success = await _userService.ChangePasswordAsync(dto.UserId, dto.CurrentPassword, dto.NewPassword);
            if (!success)
                return BadRequest("Thay đổi mật khẩu thất bại.");
            return Ok("Thay đổi mật khẩu thành công.");
        }

        [HttpPost("resend-otp")]
        public async  Task<IActionResult> ResendOtp([FromBody] SendOtpDTO dto)
        {
            try
            {
                string otp = await _otpService.SendOtp(dto.Identifier);
                return Ok(new { Message = "OTP mới đã được gửi thành công." });
            }
            catch
            {
                return BadRequest("Không thể gửi lại OTP.");
            }
        }

        // [HttpPost("refresh-token")]
        // [AllowAnonymous]
        // public async Task<IActionResult> RefreshToken()
        // {
        //     var refreshToken = Request.Headers["Authorization"].ToString();
        //     var tokenResponse = await _jwtTokenProvider.RefreshTokenAsync(refreshToken, _userRepository);

        //     if (tokenResponse != null)
        //         return Ok(tokenResponse);

        //     return Unauthorized("Refresh token không hợp lệ hoặc đã hết hạn.");
        // }

        // Đăng xuất
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO logoutDTO)
        {
            await _userService.LogoutAsync(logoutDTO.UserId);
            return Ok("Đăng xuất thành công.");
        }
    }
}