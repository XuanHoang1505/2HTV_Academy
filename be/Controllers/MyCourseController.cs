using App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("api/my-courses")]
public class MyCourseController : ControllerBase
{
    private readonly IMyCourseService _myCourseService;

    public MyCourseController(IMyCourseService myCourseService)
    {
        _myCourseService = myCourseService;
    }

    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetByStudentId(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            return BadRequest(new { message = "studentId is required." });
        }

        var courses = await _myCourseService.GetByStudentIdAsync(studentId);
        return Ok(courses);
    }
}

