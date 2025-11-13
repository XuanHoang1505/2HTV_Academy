using App.DTOs;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using AutoMapper;

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
}

