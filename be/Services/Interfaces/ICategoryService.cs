using App.DTOs;

namespace App.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> CreateAsync(CategoryDTO dto);
        Task<CategoryDTO> UpdateAsync(int id, CategoryDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
