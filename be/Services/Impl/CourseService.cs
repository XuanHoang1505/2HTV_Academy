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

        public async Task<IEnumerable<UserDTO>> GetStudentsByCourseIdAsync(int courseId)
        {
            var existingCourse = await _repository.GetByIdAsync(courseId);
            if (existingCourse == null)
            {
                throw new AppException(ErrorCode.CourseNotFound, $"Không tìm thấy khóa học với ID = {courseId}");
            }

            var students = await _repository.GetStudentsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<UserDTO>>(students);
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

        public async Task RevokeStudentAccessAsync(int courseId, string studentId)
        {
            // Kiểm tra khóa học tồn tại
            var course = await _repository.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.CourseNotFound,
                    $"Không tìm thấy khóa học với ID = {courseId}");
            }

            // Kiểm tra học viên có đang thuộc khóa này không
            var enrolled = await _repository.IsStudentEnrolledAsync(studentId, courseId);
            if (!enrolled)
            {
                throw new AppException(ErrorCode.ResourceNotFound,
                    $"Học viên với ID = {studentId} không thuộc khóa học {courseId}");
            }

            // Gỡ học viên khỏi khóa
            var removed = await _repository.RemoveStudentFromCourseAsync(studentId, courseId);
            if (!removed)
            {
                throw new AppException(ErrorCode.InternalServerError,
                    "Không thể thu hồi quyền truy cập khóa học do lỗi hệ thống.");
            }

            // Xóa tiến độ học nếu có
            await _repository.RemoveCourseProgressForStudentAsync(studentId, courseId);
        }

        public async Task<IEnumerable<CourseDTO>> SearchAsync(CourseFilterDTO filter)
        {
            var courses = await _repository.SearchAsync(filter);
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

    }
}
