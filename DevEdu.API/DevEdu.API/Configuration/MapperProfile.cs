using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;

namespace DevEdu.API.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CourseInputModel, CourseDto>().ReverseMap();
            CreateMap<GroupInputModel, GroupDto>().ReverseMap();
        }
    }
}
