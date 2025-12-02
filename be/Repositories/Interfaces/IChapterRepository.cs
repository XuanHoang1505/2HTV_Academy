using App.Domain.Models;

namespace App.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        Task<Chapter?> GetByIdAsync(int id);
        Task<Chapter?> GetByTitleAsync(string chapterTitle);
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<Chapter> AddAsync(Chapter category);
        Task UpdateAsync(Chapter category);
        Task DeleteAsync(int id);
    }
}