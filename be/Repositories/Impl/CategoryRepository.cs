    using App.Data;
    using App.Domain.Models;
    using App.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    namespace App.Repositories.Implementations
    {
        public class CategoryRepository : ICategoryRepository
        {
            private readonly AppDBContext _context;

            public CategoryRepository(AppDBContext context)
            {
                _context = context;
            }

            public async Task<Category?> GetByIdAsync(int id)
            {
                return await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            public async Task<IEnumerable<Category>> GetAllAsync()
            {
                return await _context.Categories
                    .ToListAsync();
            }

            public async Task<Category> AddAsync(Category category)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category;
            }

            public async Task UpdateAsync(Category category)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
