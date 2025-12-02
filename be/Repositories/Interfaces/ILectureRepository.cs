using App.Domain.Models;

namespace App.Repositories.Interfaces
{
    public interface ILectureRepository
    {
        Task<Lecture?> GetByIdAsync(int id);
        Task<Lecture?> GetByTitleAsync(string lectureTitle);
        Task<IEnumerable<Lecture>> GetAllAsync();
        Task<Lecture> AddAsync(Lecture lecture);
        Task UpdateAsync(Lecture lecture);
        Task DeleteAsync(int id);
    }
}