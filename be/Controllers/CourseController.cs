using App.DTOs;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {

        private readonly ICourseService _courseservice;
        public CourseController(ICourseService courseservice)
        {
            _courseservice = courseservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            var courses = await _courseservice.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCourse(int id)
        {
            var course = await _courseservice.GetByIdAsync(id);
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseDTO dto)
        {
            var course = await _courseservice.CreateAsync(dto);
            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDTO dto)
        {
            var course = await _courseservice.UpdateAsync(id, dto);
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseservice.DeleteAsync(id);
            return Ok(course);
        }
    }
}
