using App.Domain.Models;
using X.PagedList;

namespace App.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        Task<Chapter?> GetByIdAsync(int id);
        Task<Chapter?> GetByTitleAsync(string chapterTitle);
        Task<IPagedList<Chapter>> GetAllAsync(int page, int limit);
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<Chapter> AddAsync(Chapter category);
        Task UpdateAsync(Chapter category);
        Task DeleteAsync(int id);
    }
}