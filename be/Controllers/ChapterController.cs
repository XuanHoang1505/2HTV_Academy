using App.DTOs;
using App.Services.Interfaces;
using App.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/chapters")]
    public class ChapterController : ControllerBase
    {
        readonly IChapterService _chapterService;
        public ChapterController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChapters()
        {
            var chapters = await _chapterService.GetAllAsync();
            if (chapters == null)
            {
                return NotFound(new { message = "Chapter not found" });
            }
            return Ok(chapters);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterByID(int id)
        {
            var chapter = await _chapterService.GetByIdAsync(id);
            return Ok(chapter);
        }
        [HttpPost]
        public async Task<IActionResult> CreateChapter([FromBody] ChapterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _chapterService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetChapterByID),
                new { id = created.Id },
                created
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChapter(int id, [FromBody] ChapterDTO dto)
        {
            try
            {
                var updatedChapter = await _chapterService.UpdateAsync(id, dto);
                return Ok(updatedChapter);
            }
            catch (AppException ex) // nếu bạn dùng AppException để báo lỗi
            {
                return BadRequest(new { message = ex.Message, code = ex.ErrorCode });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(int id)
        {
            try
            {
                await _chapterService.DeleteAsync(id);
                return NoContent(); // 204 No Content là chuẩn cho delete thành công
            }
            catch (AppException ex) // nếu chapter không tồn tại hoặc lỗi business
            {
                return NotFound(new { message = ex.Message, code = ex.ErrorCode });
            }
        }
        
    }
}

