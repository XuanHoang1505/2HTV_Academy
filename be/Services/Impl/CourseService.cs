using AutoMapper;
using App.DTOs;
using App.Domain.Models;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using App.Utils.Exceptions;

namespace App.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CourseDTO> GetByIdAsync(int id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy khóa học với ID = {id}");

            return _mapper.Map<CourseDTO>(course);
        }


        public async Task<IEnumerable<CourseDTO>> GetAllAsync()
        {
            var courses = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> CreateAsync(CourseDTO dto)
        {
            var entity = _mapper.Map<Course>(dto);

            var created = await _repository.AddAsync(entity);
            var dtoResult = _mapper.Map<CourseDTO>(created);

            return dtoResult;
        }

        public async Task<CourseDTO> UpdateAsync(int id, CourseDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy danh mục với ID = {id}");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);

            var dtoResult = _mapper.Map<CourseDTO>(existing);
            return dtoResult;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy danh mục với ID = {id}");

            await _repository.DeleteAsync(id);
            return true;
        }

    }
}
