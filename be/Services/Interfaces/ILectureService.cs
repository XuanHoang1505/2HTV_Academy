using App.Domain.Models;
using App.DTOs;

namespace App.Services.Interfaces
{
    public interface ILectureService
    {
        Task<LectureDTO?> GetByIdAsync(int id);
        Task<LectureDTO?> GetByTitleAsync(String lectureTitle);
        Task<IEnumerable<LectureDTO>> GetAllAsync();
        Task<LectureDTO> CreateAsync(LectureDTO dto);
        Task<LectureDTO> UpdateAsync(int id, LectureDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
