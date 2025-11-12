using App.Domain.Models;
using App.DTOs;

namespace App.Services.Interfaces
{
    public interface IChapterService
    {
        Task<ChapterDTO?> GetByIdAsync(int id);
        Task<ChapterDTO?> GetByTitleAsync(String chapterTitle);
        Task<IEnumerable<ChapterDTO>> GetAllAsync();
        Task<ChapterDTO> CreateAsync(ChapterDTO dto);
        Task<ChapterDTO> UpdateAsync(int id, ChapterDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
