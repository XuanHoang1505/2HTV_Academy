using App.Data;
using App.Domain.Models;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace App.Repositories.Implementations
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly AppDBContext _context;

        public ChapterRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Chapter> AddAsync(Chapter chapter)
        {
            await _context.Chapters.AddAsync(chapter);
            await _context.SaveChangesAsync();
            return chapter;
        }

        public async Task DeleteAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            return await _context.Chapters
                .ToListAsync();
        }

        public async Task<Chapter?> GetByIdAsync(int id)
        {
            return await _context.Chapters
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Chapter?> GetByTitleAsync(string chapterTitle)
        {
            return await _context.Chapters
                .FirstOrDefaultAsync(c => c.ChapterTitle == chapterTitle);
        }

        public async Task UpdateAsync(Chapter chapter)
        {
             _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();
        }
        }
    }
