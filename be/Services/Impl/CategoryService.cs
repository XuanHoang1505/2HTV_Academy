    using AutoMapper;
    using App.DTOs;
    using App.Domain.Models;
    using App.Repositories.Interfaces;
    using App.Services.Interfaces;
    using App.Utils.Exceptions;

    namespace App.Services.Implementations
    {
        public class CategoryService : ICategoryService
        {
            private readonly ICategoryRepository _repository;
            private readonly IMapper _mapper;

            public CategoryService(ICategoryRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<CategoryDTO?> GetByIdAsync(int id)
            {
                var category = await _repository.GetByIdAsync(id);
                if (category == null)
                    throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy danh mục với ID = {id}");

                return _mapper.Map<CategoryDTO>(category);
            }


            public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
            {
                var categories = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            }

            public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
            {
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing != null)
                    throw new AppException(ErrorCode.CategorySlugAlreadyExists, "danh mục đã tồn tại.");

                var entity = _mapper.Map<Category>(dto);

                var created = await _repository.AddAsync(entity);
                var dtoResult = _mapper.Map<CategoryDTO>(created);

                return dtoResult;
            }

            public async Task<CategoryDTO> UpdateAsync(int id, CategoryDTO dto)
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy danh mục với ID = {id}");

                _mapper.Map(dto, existing);
                await _repository.UpdateAsync(existing);

                var dtoResult = _mapper.Map<CategoryDTO>(existing);

                return dtoResult;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy danh mục với ID = {id}");

                await _repository.DeleteAsync(id);
                return true;
            }

        }
    }
