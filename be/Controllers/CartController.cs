using System.Security.Claims;
using App.DTOs;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/carts")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var cartDto = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cartDto);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetCartSummary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var summary = await _cartService.GetCartSummaryAsync(userId);
            return Ok(summary);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _cartService.AddCourseToCartAsync(userId, request.CourseId);
            return Ok(new { message = "Đã thêm khóa học vào giỏ hàng thành công" });
        }

        [HttpDelete("remove/{courseId}")]
        public async Task<IActionResult> RemoveFromCart(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _cartService.RemoveCourseFromCartAsync(userId, courseId);
            return Ok(new { message = "Đã xóa khóa học khỏi giỏ hàng" });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _cartService.ClearCartAsync(userId);
            return Ok(new { message = "Đã xóa toàn bộ giỏ hàng" });
        }

        [HttpGet("check/{courseId}")]
        public async Task<IActionResult> CheckCourseInCart(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isInCart = await _cartService.IsCourseInCartAsync(userId, courseId);
            return Ok(new { isInCart });
        }
    }
}