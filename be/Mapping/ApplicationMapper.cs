using App.Data;
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
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();

            CreateMap<ApplicationUser, RegisterDTO>().ReverseMap();

            CreateMap<Cart, CartDTO>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.CartItems.Sum(ci => ci.Price)))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.CartItems.Count));

            // CartItem -> CartItemDto
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseTitle))
                .ForMember(dest => dest.CourseImage, opt => opt.MapFrom(src => src.Course.CourseThumbnail));
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
