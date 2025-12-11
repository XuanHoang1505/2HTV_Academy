using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.DTOs;
using App.Services.Interfaces;

namespace App.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(
            IReviewService reviewService,
            ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }


        public string? GetUserId()
        {
            return User.FindFirst("userId")?.Value;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDTO dto)
        {
            try
            {
                var userId = GetUserId();

                dto.UserId = userId;

                var review = await _reviewService.CreateReviewAsync(dto);
                return CreatedAtAction(nameof(GetReviewById), new { id = review.Id },
                    new { success = true, data = review });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                return Ok(new { success = true, data = review });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting review {id}");
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewService.GetAllReviewsAsync();
                return Ok(new { success = true, data = reviews });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all reviews");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetReviewsByCourse(int courseId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByCourseIdAsync(courseId);
                return Ok(new { success = true, data = reviews });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reviews for course {courseId}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsByUser(string userId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
                return Ok(new { success = true, data = reviews });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reviews for user {userId}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("course/{courseId}/stats")]
        public async Task<IActionResult> GetCourseReviewStats(int courseId)
        {
            try
            {
                var stats = await _reviewService.GetCourseReviewStatsAsync(courseId);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting review stats for course {courseId}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDTO dto)
        {
            try
            {
                var userId = GetUserId();
                var review = await _reviewService.UpdateReviewAsync(id, dto, userId);
                return Ok(new { success = true, data = review });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating review {id}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _reviewService.DeleteReviewAsync(id, userId);
                return Ok(new { success = true, message = "Xóa đánh giá thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting review {id}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpGet("check/{courseId}")]
        [Authorize]
        public async Task<IActionResult> CheckUserHasReviewed(int courseId)
        {
            try
            {
                var userId = GetUserId();
                var hasReviewed = await _reviewService.UserHasReviewedCourseAsync(userId, courseId);
                return Ok(new { success = true, hasReviewed = hasReviewed });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking review for course {courseId}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")] 
        [HttpPatch("{id}/hide")]
        public async Task<IActionResult> HideReview(int id)
        {
            var result = await _reviewService.HideReviewAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/show")]
        public async Task<IActionResult> ShowReview(int id)
        {
            var result = await _reviewService.ShowReviewAsync(id);
            return Ok(result);
        }
    }
}