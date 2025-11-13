using App.Domain.Models;
using App.DTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace footballnew.Mappings
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Chapter, ChapterDTO>().ReverseMap();
            CreateMap<Lecture, LectureDTO>().ReverseMap();

            CreateMap<Course, MyCourseDTO>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.EducatorName,
                    opt => opt.MapFrom(src => src.Educator.FullName))
                .ForMember(dest => dest.TotalChapters,
                    opt => opt.MapFrom(src => src.CourseContent.Count))
                .ForMember(dest => dest.TotalLectures,
                    opt => opt.MapFrom(src => src.CourseContent
                        .SelectMany(ch => ch.ChapterContent)
                        .Count()))
                .ForMember(dest => dest.Chapters,
                    opt => opt.MapFrom(src => src.CourseContent));
        }
    }
}

