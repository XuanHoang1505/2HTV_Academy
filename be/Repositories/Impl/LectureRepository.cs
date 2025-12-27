using App.Data;
using App.Domain.Models;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;
namespace App.Repositories.Implementations
{
    public class LectureRepository : ILectureRepository
    {
        private readonly AppDBContext _context;

        public LectureRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Lecture> AddAsync(Lecture lecture)
        {
            await _context.Lectures.AddAsync(lecture);
            await _context.SaveChangesAsync();
            return lecture;
        }

        public async Task DeleteAsync(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture != null)
            {
                _context.Lectures.Remove(lecture);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Lecture>> AllAsync()
        {
            return await _context.Lectures
                .ToListAsync();
        }

        public async Task<Lecture?> GetByIdAsync(int id)
        {
            return await _context.Lectures
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lecture?> GetByTitleAsync(string lectureTitle)
        {
            return await _context.Lectures
               .FirstOrDefaultAsync(l => l.LectureTitle == lectureTitle);
        }

        public async Task UpdateAsync(Lecture lecture)
        {
            _context.Lectures.Update(lecture);
            await _context.SaveChangesAsync();
        }

        public async Task<IPagedList<Lecture>> GetAllAsync(int page, int limit)
        {
            return await _context.Lectures
                .ToPagedListAsync(page, limit);
        }
    }
}