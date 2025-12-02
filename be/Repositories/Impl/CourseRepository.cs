using App.Data;
using App.Domain.Models;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            return await _context.Courses.Include(c => c.Category).Include(u => u.Educator)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.Include(c => c.Category).Include(u => u.Educator)
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

        public async Task<Course?> CourseDetailAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Educator)
                .Include(c => c.Category)
                .Include(c => c.CourseContent)
                .Include(c => c.CourseRatings)
                .Include(c => c.PurchaseItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            return course;
        }

        public async Task<IEnumerable<ApplicationUser>> GetStudentsByCourseIdAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.EnrolledStudents)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            return course?.EnrolledStudents ?? Enumerable.Empty<ApplicationUser>();
        }

        public async Task<IEnumerable<CourseProgress>> GetCourseProgressByCourseIdAsync(int courseId)
        {
            return await _context.CourseProgresses
                .Include(cp => cp.User)
                .Where(cp => cp.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<bool> IsStudentEnrolledAsync(string studentId, int courseId)
        {
            return await _context.Courses
                .Where(c => c.Id == courseId)
                .AnyAsync(c => c.EnrolledStudents.Any(s => s.Id == studentId));
        }

        public async Task<bool> RemoveStudentFromCourseAsync(string studentId, int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.EnrolledStudents)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return false;
            }

            var student = course.EnrolledStudents.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return false;
            }

            course.EnrolledStudents.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveCourseProgressForStudentAsync(string studentId, int courseId)
        {
            var progresses = await _context.CourseProgresses
                .Where(cp => cp.UserId == studentId && cp.CourseId == courseId)
                .ToListAsync();

            if (progresses.Count == 0)
            {
                return;
            }

            _context.CourseProgresses.RemoveRange(progresses);
            await _context.SaveChangesAsync();
        }
    }
}
