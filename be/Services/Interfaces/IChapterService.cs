using App.Domain.Models;
using App.DTOs;
using X.PagedList;

namespace App.Services.Interfaces
{
    public interface IChapterService
    {
        Task<ChapterDTO?> GetByIdAsync(int id);
        Task<ChapterDTO?> GetByTitleAsync(string chapterTitle);
        Task<PagedResult<ChapterDTO>> GetAllAsync(int? page, int? limit);
        Task<ChapterDTO> CreateAsync(ChapterDTO dto);
        Task<ChapterDTO> UpdateAsync(int id, ChapterDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
