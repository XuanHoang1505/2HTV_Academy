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
        public async Task<IActionResult> GetAllLectures(int? page, int? limit)
        {
            var lectures = await _lectureService.GetAllAsync(page, limit);
            if (lectures == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = (object?)null,
                    message = "Lecture not found"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách lecture thành công",
                data = lectures.Data,
                pagination = new
                {
                    total = lectures.Total,
                    totalPages = page.HasValue ? lectures.TotalPages : null,
                    currentPage = page.HasValue ? lectures.CurrentPage : null,
                    limit = page.HasValue ? lectures.Limit : null
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLectureByID(int id)
        {
            var lecture = await _lectureService.GetByIdAsync(id);
            if (lecture == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = (object?)null,
                    message = "Lecture not found"
                });
            }
            return Ok(new
            {
                success = true,
                data = lecture,
                message = "Lấy thông tin lecture thành công"
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateLecture([FromBody] LectureDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    data = (object?)null,
                    message = "Dữ liệu không hợp lệ"
                });

            var created = await _lectureService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetLectureByID),
                new { id = created.Id },
                new
                {
                    success = true,
                    data = created,
                    message = "Tạo lecture thành công"
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecture(int id, [FromBody] LectureDTO dto)
        {
            try
            {
                var updatedLecture = await _lectureService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    data = updatedLecture,
                    message = "Cập nhật lecture thành công"
                });
            }
            catch (AppException ex) // nếu bạn dùng AppException để báo lỗi
            {
                return BadRequest(new
                {
                    success = false,
                    data = (object?)null,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            try
            {
                await _lectureService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    data = (object?)null,
                    message = "Xóa lecture thành công"
                });
            }
            catch (AppException ex) // nếu lecture không tồn tại hoặc lỗi business
            {
                return NotFound(new
                {
                    success = false,
                    data = (object?)null,
                    message = ex.Message
                });
            }
        }
    }

}
