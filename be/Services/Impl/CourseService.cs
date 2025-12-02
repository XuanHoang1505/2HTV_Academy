using AutoMapper;
using App.DTOs;
using App.Domain.Models;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using App.Utils.Exceptions;
using App.Services;

namespace App.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinaryService;


        public CourseService(ICourseRepository repository, IMapper mapper, CloudinaryService cloudinaryService)
        {
            _repository = repository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
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

            if (dto.CourseThumbnailFile != null && dto.CourseThumbnailFile.Length > 0)
            {
                entity.CourseThumbnail = await _cloudinaryService.UploadImageAsync(dto.CourseThumbnailFile, "course_thumbnail");
            }

            var created = await _repository.AddAsync(entity);
            return _mapper.Map<CourseDTO>(created);
        }

        public async Task<CourseDTO> UpdateAsync(int id, CourseDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy khóa học với ID = {id}");

            if (dto.CourseThumbnailFile != null && dto.CourseThumbnailFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existing.CourseThumbnail))
                {
                    var oldPublicId = CloudinaryService.ExtractPublicId(existing.CourseThumbnail);
                    await _cloudinaryService.DeleteImageAsync(oldPublicId);
                }
                existing.CourseThumbnail = await _cloudinaryService.UploadImageAsync(dto.CourseThumbnailFile, "course_thumbnail");
            }


            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);

            return _mapper.Map<CourseDTO>(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy khóa học với ID = {id}");

            if (!string.IsNullOrEmpty(existing.CourseThumbnail))
            {
                var publicId = CloudinaryService.ExtractPublicId(existing.CourseThumbnail);
                await _cloudinaryService.DeleteImageAsync(publicId);
            }

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<CourseDetailDTO?> CourseDetailAsync(int id)
        {
            var course = await _repository.CourseDetailAsync(id);
            if (course == null)
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy khóa học với ID = {id}");

            return _mapper.Map<CourseDetailDTO>(course);
        }

    }
}
