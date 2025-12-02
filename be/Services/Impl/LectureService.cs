using AutoMapper;
using App.DTOs;
using App.Domain.Models;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using App.Utils.Exceptions;

namespace App.Services.Implementations
{
    public class LectureService : ILectureService
    {
        private readonly ILectureRepository _lecture;
        private readonly IMapper _mapper;
        public LectureService(ILectureRepository lecture, IMapper mapper)
        {
            _lecture = lecture;
            _mapper = mapper;
        }

        public async Task<LectureDTO> CreateAsync(LectureDTO dto)
        {
            var existing = await _lecture.GetByTitleAsync(dto.LectureTitle);
            if (existing != null)
                throw new AppException(ErrorCode.CategorySlugAlreadyExists, $"Slug '{dto.LectureTitle}' đã tồn tại.");
            
            var entity = _mapper.Map<Lecture>(dto);

            var created = await _lecture.AddAsync(entity);
            var dtoResult = _mapper.Map<LectureDTO>(created);

            return dtoResult;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _lecture.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy bài học với ID = {id}");

            await _lecture.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<LectureDTO>> GetAllAsync()
        {
            var lectures = await _lecture.GetAllAsync();
            return _mapper.Map<IEnumerable<LectureDTO>>(lectures);
        }

        public async Task<LectureDTO?> GetByIdAsync(int id)
        {
            var lecture = await _lecture.GetByIdAsync(id);
            if (lecture == null)
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy bài học với ID = {id}");

            return _mapper.Map<LectureDTO>(lecture);
        }

        public async Task<LectureDTO?> GetByTitleAsync(string lectureTitle)
        {
            var lecture = await _lecture.GetByTitleAsync(lectureTitle);
            if (lecture == null)
            {
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy bài học với ID = {lectureTitle}");
            }
            return _mapper.Map<LectureDTO>(lecture);
        }

        public async Task<LectureDTO> UpdateAsync(int id, LectureDTO dto)
        {
            var existing = await _lecture.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy bài học với ID = {id}");

            _mapper.Map(dto, existing);
            await _lecture.UpdateAsync(existing);

            var dtoResult = _mapper.Map<LectureDTO>(existing);
            return dtoResult;
        }
    }
}