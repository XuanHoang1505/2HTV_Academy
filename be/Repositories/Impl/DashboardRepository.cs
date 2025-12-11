using App.Data;
using App.Domain.Enums;
using App.DTOs;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Implementations
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDBContext _context;

        public DashboardRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<DashboardOverviewDTO> GetDashboardOverview()
        {
            var result = new DashboardOverviewDTO();

            // Tổng khóa học
            result.TotalCourses = await _context.Courses.CountAsync();

            // Tổng học viên (unique)
            result.TotalStudents = await _context.Enrollments
                .Select(e => e.UserId)
                .Distinct()
                .CountAsync();

            // Tổng doanh thu
            result.TotalRevenue =
                await _context.Purchases
                    .Where(p => p.Status == PurchaseStatus.Completed)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // Doanh thu theo tháng
            result.MonthlyRevenue = await _context.Purchases
                .Where(p => p.Status == PurchaseStatus.Completed)
                .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
                .Select(g => new MonthlyRevenueDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            // Thống kê khóa học
            var stats = await _context.Courses
                .Select(c => new CourseStatsDTO
                {
                    Id = c.Id,
                    CourseTitle = c.CourseTitle,
                    Students = c.Enrollments.Count,
                    Revenue = c.PurchaseItems
                                .Where(pi => pi.Purchase.Status == PurchaseStatus.Completed)
                                .Sum(pi => (decimal?)pi.Price) ?? 0
                })
                .ToListAsync();

            result.CourseStats = stats;

            result.TopCourses = stats
                .OrderByDescending(s => s.Students)
                .Take(5)
                .ToList();

            return result;
        }
    }
}