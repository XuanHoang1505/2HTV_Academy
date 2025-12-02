using App.DTOs;

namespace App.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CourseDTO>> GetAllAsync();
        Task<CourseDTO> CreateAsync(CourseDTO dto);
        Task<CourseDTO> UpdateAsync(int id, CourseDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<CourseDetailDTO?> CourseDetailAsync(int id);
        Task<IEnumerable<UserDTO>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<StudentCourseProgressDTO>> GetStudentProgressByCourseIdAsync(int courseId);
        Task RevokeStudentAccessAsync(int courseId, string studentId);
    }
}
