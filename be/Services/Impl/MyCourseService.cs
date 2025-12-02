using App.DTOs;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using AutoMapper;
using App.Utils.Exceptions;

namespace App.Services.Implementations;

public class MyCourseService : IMyCourseService
{
    private readonly IMyCourseRepository _myCourseRepository;
    private readonly IMapper _mapper;

    public MyCourseService(IMyCourseRepository myCourseRepository, IMapper mapper)
    {
        _myCourseRepository = myCourseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MyCourseDTO>> GetByStudentIdAsync(string studentId)
    {
        var courses = await _myCourseRepository.GetCoursesByStudentIdAsync(studentId);
        return _mapper.Map<IEnumerable<MyCourseDTO>>(courses);
    }

    public async Task<MyCourseDTO> GetDetailByStudentAsync(string studentId, int courseId)
    {
        var course = await _myCourseRepository.GetCourseDetailAsync(studentId, courseId);
        if (course == null)
        {
            throw new AppException(ErrorCode.ResourceNotFound,
                $"The course with id '{courseId}' is not associated with the student '{studentId}'.");
        }

        return _mapper.Map<MyCourseDTO>(course);
    }
}

