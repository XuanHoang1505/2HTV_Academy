using App.Data;
using App.Domain.Models;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Implementations;

public class MyCourseRepository : IMyCourseRepository
{
    private readonly AppDBContext _context;

    public MyCourseRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(string studentId)
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Educator)
            .Include(c => c.CourseContent)
            .Where(c => c.EnrolledStudents.Any(s => s.Id == studentId))
            .ToListAsync();
    }
}

