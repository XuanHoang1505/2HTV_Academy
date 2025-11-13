using App.Domain.Models;

namespace App.Repositories.Interfaces;

public interface IMyCourseRepository
{
    Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(string studentId);
}

