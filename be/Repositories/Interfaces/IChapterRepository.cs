using App.Domain.Models;
using X.PagedList;

namespace App.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        Task<Chapter?> GetByIdAsync(int id);
        Task<Chapter?> GetByTitleAsync(string chapterTitle);
        Task<IPagedList<Chapter>> GetAllAsync(int page, int limit, Dictionary<string, string>? filters = null);
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<Chapter> AddAsync(Chapter category);
        Task UpdateAsync(Chapter category);
        Task DeleteAsync(int id);
        /// <summary>
        /// Get the next chapter order for a specific course (max existing + 1). Returns 1 when no chapter exists for the course.
        /// </summary>
        Task<int> GetNextChapterOrderAsync(int courseId);
    }
}