using App.Data;
using App.Domain.Models;
using App.DTOs;

namespace App.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<Course?> CourseDetailAsync(int id);
        Task<IEnumerable<CourseProgress>> GetCourseProgressByCourseIdAsync(int courseId);
        Task RemoveCourseProgressForStudentAsync(string studentId, int courseId);
        Task<IEnumerable<Course>> SearchAsync(CourseFilterDTO filter);
    }
}