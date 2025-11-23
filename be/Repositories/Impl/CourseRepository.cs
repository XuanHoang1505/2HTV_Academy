using App.Data;
using App.Domain.Models;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDBContext _context;

        public CourseRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.Include(c=>c.Category).Include(u=>u.Educator)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.Include(c=>c.Category).Include(u=>u.Educator)
                .ToListAsync();
        }

        public async Task<Course> AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task UpdateAsync(Course course)
        {
             _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }
    }
}
