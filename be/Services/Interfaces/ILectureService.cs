using App.Domain.Models;
using App.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Services.Interfaces
{
    public interface ILectureService
    {
        Task<LectureDTO?> GetByIdAsync(int id);
        Task<LectureDTO?> GetByTitleAsync(String lectureTitle);
        Task<PagedResult<LectureDTO>> GetAllAsync(int? page, int? limit);
        Task<LectureDTO> CreateAsync(LectureDTO dto);
        Task<LectureDTO> UpdateAsync(int id, LectureDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountLecturesAsync(int courseId);
        Task<int> TotalDurationAsync(int courseId);
    }
}
