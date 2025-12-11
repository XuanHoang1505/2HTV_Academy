using App.Domain.Models;

namespace App.Repositories.Interfaces
{
public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment?> GetByUserAndCourseAsync(string userId, int courseId);
        Task<IEnumerable<Enrollment>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseId);
        Task<Enrollment> CreateAsync(Enrollment enrollment);
        Task<Enrollment> UpdateAsync(Enrollment enrollment);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(string userId, int courseId);
        Task<int> GetTotalEnrollmentsByCourseAsync(int courseId);
        Task<IEnumerable<Enrollment>> GetActiveEnrollmentsByUserAsync(string userId);
    }
}