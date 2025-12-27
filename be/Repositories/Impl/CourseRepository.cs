using App.Data;
using App.Domain.Enums;
using App.Domain.Models;
using App.DTOs;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList;
using X.PagedList.EF;

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
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Educator)
                .Include(c => c.CourseContent.OrderBy(ch => ch.ChapterOrder))
                    .ThenInclude(ch => ch.ChapterContent.OrderBy(l => l.LectureOrder))
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetBySlugAsync(string slug)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Educator)
                .Include(c => c.CourseContent.OrderBy(ch => ch.ChapterOrder))
                    .ThenInclude(ch => ch.ChapterContent.OrderBy(l => l.LectureOrder))
                .Where(c => c.IsPublished == true && c.Status == CourseStatus.published)
                .FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<IPagedList<Course>> GetAllAsync(int page, int limit)
        {
            return await _context.Courses.Include(c => c.Category).Include(u => u.Educator).ToPagedListAsync(page, limit);

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
                .Include(c => c.PurchaseItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            return course;
        }

        // public async Task<IEnumerable<CourseProgress>> GetCourseProgressByCourseIdAsync(int courseId)
        // {
        //     return await _context.CourseProgresses
        //         .Include(cp => cp.User)
        //         .Where(cp => cp.CourseId == courseId)
        //         .ToListAsync();
        // }

        // public async Task RemoveCourseProgressForStudentAsync(string studentId, int courseId)
        // {
        //     var progresses = await _context.CourseProgresses
        //         .Where(cp => cp.UserId == studentId && cp.CourseId == courseId)
        //         .ToListAsync();

        //     if (progresses.Count == 0)
        //     {
        //         return;
        //     }

        //     _context.CourseProgresses.RemoveRange(progresses);
        //     await _context.SaveChangesAsync();
        // }

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

        public async Task<IEnumerable<Course>> GetCoursesBestSellerAsync()
        {
            return await _context.Courses
                     .Include(c => c.Category)
                     .Where(c => c.Status.Equals("published") && c.IsPublished == true)
                     .OrderByDescending(c => c.TotalStudents)
                     .Take(4)
                     .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesNewestAsync()
        {
            return await _context.Courses
                       .Include(c => c.Category)
                       .Where(c => c.Status.Equals("published") && c.IsPublished == true)
                       .OrderByDescending(c => c.CreatedAt)
                       .Take(4)
                       .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesRatingAsync()
        {
            return await _context.Courses
                       .Include(c => c.Category)
                       .Where(c => c.Status.Equals("published") && c.IsPublished == true)
                       .OrderByDescending(c => c.AverageRating)
                       .Take(4)
                       .ToListAsync();
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            return await _context.Courses
                .AnyAsync(c => c.Slug == slug);
        }

        public async Task<IPagedList<Course>> GetAllPublishAsync(int page, int limit)
        {
            return await _context.Courses.Include(c => c.Category).Include(u => u.Educator)
                 .Where(c => c.Status.Equals("published") && c.IsPublished == true).ToPagedListAsync(page, limit);
        }

        public async Task<IEnumerable<Course>> AllCoursesPublishAsync()
        {
            return await _context.Courses.Include(c => c.Category).Include(u => u.Educator)
                 .Where(c => c.Status.Equals("published") && c.IsPublished == true)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Course>> AllCoursesAsync()
        {
            return await _context.Courses.Include(c => c.Category).Include(u => u.Educator)
                .ToListAsync();
        }
    }
}
