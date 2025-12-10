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
            CreateMap<Chapter, ChapterDTO>().ReverseMap();
            CreateMap<Lecture, LectureDTO>().ReverseMap();

            CreateMap<Chapter, MyCourseChapterDTO>()
                .ForMember(dest => dest.Lectures,
                    opt => opt.MapFrom(src => src.ChapterContent));

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

            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());


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


            CreateMap<CourseDTO, Course>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.CourseThumbnail, opt => opt.Ignore());

            CreateMap<Course, CourseDTO>()
             .ForMember(dest => dest.EducatorName, opt => opt.MapFrom(src => src.Educator.FullName))
             .ForMember(dest => dest.CourseThumbnailFile, opt => opt.Ignore())
             .ForMember(dest => dest.CourseThumbnail, opt => opt.MapFrom(src => src.CourseThumbnail));

            CreateMap<Course, CourseDetailDTO>().ReverseMap();


            CreateMap<Purchase, PurchaseDTO>();
            CreateMap<PurchaseDTO, Purchase>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PurchaseItem, PurchaseItemDTO>().ReverseMap();

            


        }
    }
}

