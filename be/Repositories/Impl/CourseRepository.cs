using App.Data;
using App.Domain.Models;
using App.DTOs;
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

        public async Task<IEnumerable<CourseProgress>> GetCourseProgressByCourseIdAsync(int courseId)
        {
            return await _context.CourseProgresses
                .Include(cp => cp.User)
                .Where(cp => cp.CourseId == courseId)
                .ToListAsync();
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

        public async Task<IEnumerable<Course>> SearchAsync(CourseFilterDTO filter)
        {
            var query = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Educator)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var keyword = filter.Keyword.Trim().ToLower();
                query = query.Where(c =>
                    c.CourseTitle.ToLower().Contains(keyword) ||
                    c.CourseDescription.ToLower().Contains(keyword));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(c => c.CategoryId == filter.CategoryId.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(c => c.CoursePrice >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(c => c.CoursePrice <= filter.MaxPrice.Value);
            }

            if (filter.IsPublished.HasValue)
            {
                query = query.Where(c => c.IsPublished == filter.IsPublished.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                switch (filter.SortBy.ToLower())
                {
                    case "price":
                        query = filter.SortDesc
                            ? query.OrderByDescending(c => c.CoursePrice)
                            : query.OrderBy(c => c.CoursePrice);
                        break;
                    case "title":
                    default:
                        query = filter.SortDesc
                            ? query.OrderByDescending(c => c.CourseTitle)
                            : query.OrderBy(c => c.CourseTitle);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(c => c.CourseTitle);
            }

            return await query.ToListAsync();
        }
    }
}
