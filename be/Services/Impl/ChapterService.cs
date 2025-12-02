using AutoMapper;
using App.DTOs;
using App.Domain.Models;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using App.Utils.Exceptions;

namespace App.Services.Implementations
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapter;
        private readonly IMapper _mapper;
        public ChapterService(IChapterRepository chapter, IMapper mapper)
        {
            _chapter = chapter;
            _mapper = mapper;
        }

        public async Task<ChapterDTO> CreateAsync(ChapterDTO dto)
        {
            var existing = await _chapter.GetByTitleAsync(dto.ChapterTitle);
            if (existing != null)
                throw new AppException(ErrorCode.CategorySlugAlreadyExists, $"Slug '{dto.ChapterTitle}' đã tồn tại.");
            
            var entity = _mapper.Map<Chapter>(dto);

            var created = await _chapter.AddAsync(entity);
            var dtoResult = _mapper.Map<ChapterDTO>(created);

            return dtoResult;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _chapter.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy chương với ID = {id}");

            await _chapter.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ChapterDTO>> GetAllAsync()
        {
            var chapters = await _chapter.GetAllAsync();
            return _mapper.Map<IEnumerable<ChapterDTO>>(chapters);
        }

        public async Task<ChapterDTO?> GetByIdAsync(int id)
        {
            var chapter = await _chapter.GetByIdAsync(id);
            if (chapter == null)
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy chương với ID = {id}");

            return _mapper.Map<ChapterDTO>(chapter);
        }

        public async Task<ChapterDTO?> GetByTitleAsync(string chapterTitle)
        {
            var chapter = await _chapter.GetByTitleAsync(chapterTitle);
            if (chapter == null)
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy chương với ID = {chapterTitle}");

            return _mapper.Map<ChapterDTO>(chapter);
        }

        public async Task<ChapterDTO> UpdateAsync(int id, ChapterDTO dto)
        {
            var existing = await _chapter.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(ErrorCode.CategoryNotFound, $"Không tìm thấy chương với ID = {id}");

            _mapper.Map(dto, existing);
            await _chapter.UpdateAsync(existing);

            var dtoResult = _mapper.Map<ChapterDTO>(existing);
            return dtoResult;
        }
    }

}
