using App.DTOs;
using App.Services.Interfaces;
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
        public async Task<IActionResult> GetAllCourse()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCourse(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseDTO dto)
        {
            var userId = GetUserId();
            dto.EducatorId = userId;
            var course = await _courseService.CreateAsync(dto);
            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDTO dto)
        {
            var userId = GetUserId();
            dto.EducatorId = userId;
            var course = await _courseService.UpdateAsync(id, dto);
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseService.DeleteAsync(id);
            return Ok(course);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> CourseDetail(int id)
        {
            var course = await _courseService.CourseDetailAsync(id);
            return Ok(course);
        }

        // Lấy tất cả sinh viên đã đăng ký một khóa học
        [HttpGet("{id}/students")]
        public async Task<IActionResult> GetStudentsByCourseId(int id)
        {
            var students = await _courseService.GetStudentsByCourseIdAsync(id);
            return Ok(students);
        }

        // Tiến độ học tập của từng học viên trong khóa
        [HttpGet("{id}/students/progress")]
        public async Task<IActionResult> GetStudentProgressByCourseId(int id)
        {
            var progress = await _courseService.GetStudentProgressByCourseIdAsync(id);
            return Ok(progress);
        }

        // Thu hồi quyền truy cập khóa học của một học viên
        [HttpDelete("{courseId}/students/{studentId}")]
        public async Task<IActionResult> RevokeStudentAccess(int courseId, string studentId)
        {
            await _courseService.RevokeStudentAccessAsync(courseId, studentId);
            return NoContent();
        }
    }
}
