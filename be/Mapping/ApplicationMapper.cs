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
        }
    }
}
