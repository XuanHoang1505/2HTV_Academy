using App.DTOs;

namespace App.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDetailDTO?> GetByIdAsync(int id);
        Task<Object> GetAllCoursesPublishAsync(int? page, int? limit);
        Task<Object> GetAllCourses(int? page, int? limit);
        Task<CourseDetailDTO?> GetBySlugAsync(string slug);
        Task<CourseDTO> CreateAsync(CourseDTO dto);
        Task<CourseDTO> UpdateAsync(int id, CourseDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<CourseDetailDTO?> CourseDetailAsync(int id);
        Task<IEnumerable<StudentCourseProgressDTO>> GetStudentProgressByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseDTO>> SearchAsync(CourseFilterDTO filter);
        Task<IEnumerable<CourseDTO>> GetCoursesBestSellerAsync();
        Task<IEnumerable<CourseDTO>> GetCoursesNewestAsync();
        Task<IEnumerable<CourseDTO>> GetCoursesRatingAsync();
    }
}
