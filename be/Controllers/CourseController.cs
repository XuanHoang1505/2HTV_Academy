using App.Domain.Models;
using App.DTOs;
using App.Services.Interfaces;
using App.Utils.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/courses")]

    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public string? GetUserId()
        {
            return User.FindFirst("userId")?.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses(int? page, int? limit)
        {
            var courses = await _courseService.GetAllCourses(page, limit);
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách khóa học thành công ",
                data = courses
            }
           );
        }

        [HttpGet("courses-published")]
        public async Task<IActionResult> GetAllCoursesPublish(int? page, int? limit)
        {
            var courses = await _courseService.GetAllCoursesPublishAsync(page, limit);
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách khóa học thành công ",
                data = courses
            }
           );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCourse(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            return Ok(new
            {
                success = true,
                message = "Lấy khóa học thành công ",
                data = course
            }
            );
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse(CourseDTO dto)
        {
            var userId = GetUserId();
            dto.EducatorId = userId;
            var course = await _courseService.CreateAsync(dto);

            if (course == null)
                return NotFound(new
                {
                    success = false,
                    message = "Tạo không thành công"
                });

            return Ok(new
            {
                success = true,
                message = "Tạo khóa học thành công ",
                data = course
            }
                 );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDTO dto)
        {
            var userId = GetUserId();
            dto.EducatorId = userId;
            var course = await _courseService.UpdateAsync(id, dto);

            if (course == null)
                return NotFound(new
                {
                    success = false,
                    message = "Cập nhật không thành công"
                });

            return Ok(new
            {
                success = true,
                message = "Sửa khóa học thành công ",
                data = course
            }
                );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseService.DeleteAsync(id);

            if (!course)
                return NotFound(new
                {
                    success = false,
                    message = "Xóa thất bại"
                });

            return Ok(new
            {
                success = true,
                message = "Xóa khóa học thành công ",
                data = course
            }
                );
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> CourseDetail(int id)
        {
            var course = await _courseService.CourseDetailAsync(id);
            return Ok(new
            {
                success = true,
                message = "Lấy  chi tiết khóa học thành công ",
                data = course
            }
            );
        }

        // Tiến độ học tập của từng học viên trong khóa
        [HttpGet("{id}/students/progress")]
        public async Task<IActionResult> GetStudentProgressByCourseId(int id)
        {
            var progress = await _courseService.GetStudentProgressByCourseIdAsync(id);
            return Ok(new
            {
                success = true,
                message = "Lấy tiến trình khóa học thành công ",
                data = progress
            }
            );
        }

        // Tìm kiếm / lọc khóa học
        [HttpGet("search")]
        public async Task<IActionResult> SearchCourses(
            [FromQuery] string? keyword,
            [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? isPublished,
            [FromQuery] string? sortBy,
            [FromQuery] bool sortDesc = false)
        {
            try
            {
                var filter = new CourseFilterDTO
                {
                    Keyword = keyword,
                    CategoryId = categoryId,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    IsPublished = isPublished,
                    SortBy = sortBy,
                    SortDesc = sortDesc
                };

                var courses = await _courseService.SearchAsync(filter);
                return Ok(new
                {
                    success = true,
                    data = courses,
                    message = "Tìm kiếm khóa học thành công"
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    data = (object?)null,
                    message = ex.Message
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    success = false,
                    data = (object?)null,
                    message = "Đã xảy ra lỗi khi tìm kiếm khóa học"
                });
            }
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetCourseBySlug(string slug)
        {
            var course = await _courseService.GetBySlugAsync(slug);
            return Ok(new
            {
                success = true,
                message = "Lấy khóa học thành công ",
                data = course
            }
            );
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetCoursesBestSeller()
        {
            var courses = await _courseService.GetCoursesBestSellerAsync();
            return Ok(new
            {
                success = true,
                message = "Lấy các khóa học bán chạy thành công ",
                data = courses
            }
           );
        }

        [HttpGet("newest")]
        public async Task<IActionResult> GetCoursesNewest()
        {
            var courses = await _courseService.GetCoursesNewestAsync();
            return Ok(new
            {
                success = true,
                message = "Lấy các khóa học mới nhất thành công ",
                data = courses
            }
          );
        }

        [HttpGet("rating")]
        public async Task<IActionResult> GetCoursesRating()
        {
            var courses = await _courseService.GetCoursesRatingAsync();
            return Ok(new
            {
                success = true,
                message = "Lấy cáo khóa học được đánh giá cao thành công ",
                data = courses
            }
            );
        }
    }
}
