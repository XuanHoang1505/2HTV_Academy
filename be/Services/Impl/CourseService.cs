using AutoMapper;
using App.DTOs;
using App.Domain.Models;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using App.Utils.Exceptions;
using App.Services;
using App.Domain.Enums;

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


        public async Task<CourseDTO> CreateAsync(CourseDTO dto)
        {
            var slugExists = await _repository.ExistsBySlugAsync(dto.Slug);
            if (slugExists)
                throw new AppException(
                    ErrorCode.SlugAlreadyExists,
                    $"Slug '{dto.Slug}' đã tồn tại"
                );

            var entity = _mapper.Map<Course>(dto);

            if (dto.CourseThumbnailFile != null && dto.CourseThumbnailFile.Length > 0)
            {
                entity.CourseThumbnail = await _cloudinaryService
                    .UploadImageAsync(dto.CourseThumbnailFile, "course_thumbnail");
            }

            if (dto.Status.Equals("published"))
            {
                entity.Status = CourseStatus.published;
                entity.IsPublished = true;
                entity.PublishedAt = DateTime.UtcNow;
            }
            else
            {
                entity.Status = CourseStatus.draft;
                entity.IsPublished = false;
                entity.PublishedAt = null;
            }

            var created = await _repository.AddAsync(entity);
            return _mapper.Map<CourseDTO>(created);
        }

        public async Task<CourseDTO> UpdateAsync(int id, CourseDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new AppException(
                    ErrorCode.CourseNotFound,
                    $"Không tìm thấy khóa học với ID = {id}"
                );

            if (dto.CourseThumbnailFile != null && dto.CourseThumbnailFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existing.CourseThumbnail))
                {
                    var oldPublicId = CloudinaryService.ExtractPublicId(existing.CourseThumbnail);
                    await _cloudinaryService.DeleteImageAsync(oldPublicId);
                }

                existing.CourseThumbnail = await _cloudinaryService
                    .UploadImageAsync(dto.CourseThumbnailFile, "course_thumbnail");
            }

            _mapper.Map(dto, existing);

            if (dto.Status.Equals("published"))
            {
                existing.Status = CourseStatus.published;
                existing.IsPublished = true;
                if (existing.PublishedAt == null)
                {
                    existing.PublishedAt = DateTime.UtcNow;
                }
            }

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


        public async Task<IEnumerable<StudentCourseProgressDTO>> GetStudentProgressByCourseIdAsync(int courseId)
        {
            var course = await _repository.CourseDetailAsync(courseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy khóa học với ID = {courseId}");
            }

            // Tổng số bài giảng trong khóa
            var totalLectures = course.CourseContent
                .SelectMany(ch => ch.ChapterContent)
                .Count();

            if (totalLectures == 0)
            {
                // Nếu chưa có lecture, trả về danh sách rỗng
                return Enumerable.Empty<StudentCourseProgressDTO>();
            }

            var progresses = await _repository.GetCourseProgressByCourseIdAsync(courseId);

            var result = progresses.Select(cp =>
            {
                var completedLectureCount = 0;
                if (!string.IsNullOrWhiteSpace(cp.LectureCompleted))
                {
                    completedLectureCount = cp.LectureCompleted
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Length;
                }

                var percent = totalLectures == 0
                    ? 0
                    : (double)completedLectureCount / totalLectures * 100.0;

                return new StudentCourseProgressDTO
                {
                    UserId = cp.UserId,
                    FullName = cp.User.FullName,
                    Email = cp.User.Email,
                    CourseId = cp.CourseId,
                    Completed = cp.Completed,
                    TotalLectures = totalLectures,
                    CompletedLectures = completedLectureCount,
                    ProgressPercent = Math.Round(percent, 2),
                    LectureCompletedRaw = cp.LectureCompleted
                };
            });

            return result;
        }



        public async Task<IEnumerable<CourseDTO>> SearchAsync(CourseFilterDTO filter)
        {
            var courses = await _repository.SearchAsync(filter);
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<CourseDTO?> GetBySlugAsync(string slug)
        {
            var course = await _repository.GetBySlugAsync(slug);
            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesBestSellerAsync()
        {
            var courses = await _repository.GetCoursesBestSellerAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesNewestAsync()
        {
            var courses = await _repository.GetCoursesNewestAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesRatingAsync()
        {
            var courses = await _repository.GetCoursesRatingAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<object> GetAllCoursesPublishAsync(int? page, int? limit)
        {
            if (!page.HasValue || !limit.HasValue)
            {
                var allCorsesPublish = await _repository.AllCoursesPublishAsync();
                var totalCourses = allCorsesPublish.Count();

                return new
                {
                    data = _mapper.Map<IEnumerable<CourseDTO>>(allCorsesPublish),
                    total = totalCourses
                };
            }

            var courses = await _repository.GetAllPublishAsync(page.Value, limit.Value);

            return new
            {
                data = _mapper.Map<IEnumerable<CourseDTO>>(courses),
                total = courses.TotalItemCount,
                totalPages = courses.PageCount,
                currentPage = courses.PageNumber,
                limit = courses.PageSize
            };
        }

        public async Task<object> GetAllCourses(int? page, int? limit)
        {
            if (!page.HasValue || !limit.HasValue)
            {
                var allCorses = await _repository.AllCoursesAsync();
                var totalCourses = allCorses.Count();

                return new
                {
                    data = _mapper.Map<IEnumerable<CourseDTO>>(allCorses),
                    total = totalCourses
                };
            }

            var courses = await _repository.GetAllAsync(page.Value, limit.Value);

            return new
            {
                data = _mapper.Map<IEnumerable<CourseDTO>>(courses),
                total = courses.TotalItemCount,
                totalPages = courses.PageCount,
                currentPage = courses.PageNumber,
                limit = courses.PageSize
            };
        }
    }
}
