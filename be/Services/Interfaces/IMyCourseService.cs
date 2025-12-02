using App.DTOs;

namespace App.Services.Interfaces;

public interface IMyCourseService
{
    Task<IEnumerable<MyCourseDTO>> GetByStudentIdAsync(string studentId);
    Task<MyCourseDTO> GetDetailByStudentAsync(string studentId, int courseId);
}

