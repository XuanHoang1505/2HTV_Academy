using App.DTOs;
using App.Services.Interfaces;
using App.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/lectures")]
    public class LectureController : ControllerBase
    {
        readonly ILectureService _lectureService;
        public LectureController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLectures()
        {
            var lectures = await _lectureService.GetAllAsync();
            if (lectures == null)
            {
                return NotFound(new { message = "Lecture not found" });
            }
            return Ok(lectures);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLectureByID(int id)
        {
            var lecture = await _lectureService.GetByIdAsync(id);
            return Ok(lecture);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLecture([FromBody] LectureDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _lectureService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetLectureByID),
                new { id = created.Id },
                created
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecture(int id, [FromBody] LectureDTO dto)
        {
            try
            {
                var updatedLecture = await _lectureService.UpdateAsync(id, dto);
                return Ok(updatedLecture);
            }
            catch (AppException ex) // nếu bạn dùng AppException để báo lỗi
            {
                return BadRequest(new { message = ex.Message, code = ex.ErrorCode });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            try
            {
                await _lectureService.DeleteAsync(id);
                return NoContent(); // 204 No Content là chuẩn cho delete thành công
            }
            catch (AppException ex) // nếu chapter không tồn tại hoặc lỗi business
            {
                return NotFound(new { message = ex.Message, code = ex.ErrorCode });
            }
        }
    } 
    
}
