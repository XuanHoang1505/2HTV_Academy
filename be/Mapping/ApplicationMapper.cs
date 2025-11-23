using App.Domain.Models;
using App.DTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace App.Mappings
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Category, CategoryDTO>(); 
            CreateMap<CategoryDTO, Category>()  
             .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<Course, CourseDTO>()
             .ForMember(dest => dest.EducatorName, opt => opt.MapFrom(src => src.Educator.FullName));
            CreateMap<CourseDTO, Course>()
             .ForMember(dest => dest.Id, opt => opt.Ignore());

    }
    }
}
