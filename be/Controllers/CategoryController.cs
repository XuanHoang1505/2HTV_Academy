using App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using App.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] int? page, [FromQuery] int? limit)
        {
            var result = await _categoryService.GetAllAsync(page, limit);

            return Ok(new
            {
                success = true,
                message = "Lấy danh sách danh mục thành công",
                data = result.Data,
                pagination = new
                {
                    total = result.Total,
                    totalPages = page.HasValue ? result.TotalPages : null,
                    currentPage = page.HasValue ? result.CurrentPage : null,
                    limit = page.HasValue ? result.Limit : null
                }
            });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(new
            {
                success = true,
                message = "Lấy danh mục thành công ",
                data = category
            }
            );
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CategoryDTO dto)
        {
            var category = await _categoryService.CreateAsync(dto);

            if (category == null)
                return NotFound(new
                {
                    success = false,
                    message = "Tạo danh mục không thành công"
                });

            return Ok(new
            {
                success = true,
                message = "Tạo danh mục thành công ",
                data = category
            }
               );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO dto)
        {
            var category = await _categoryService.UpdateAsync(id, dto);

            if (category == null)
                return NotFound(new
                {
                    success = false,
                    message = "Cập nhật không thành công"
                });

            return Ok(new
            {
                success = true,
                message = "Sửa danh mục thành công ",
                data = category
            }
               );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.DeleteAsync(id);
            if (!category)
                return NotFound(new
                {
                    success = false,
                    message = "Xóa thất bại"
                });

            return Ok(new
            {
                success = true,
                message = "Xóa danh mục thành công ",
                data = category
            }
        );
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetCategoryBySlug(string slug)
        {
            var category = await _categoryService.GetBySlugAsync(slug);
            return Ok(new
            {
                success = true,
                message = "Lấy danh mục thành công ",
                data = category
            }
           );
        }
    }
}
