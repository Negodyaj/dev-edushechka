using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;

namespace DevEdu.API.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMappingToDto();
            CreateMappingFromDto();
        }

        private void CreateMappingToDto()
        {
            CreateMap<CourseInputModel, CourseDto>();
            CreateMap<NotificationAddInputModel, NotificationDto>();
            CreateMap<NotificationUpdateInputModel, NotificationDto>();
        }

        private void CreateMappingFromDto()
        {

        }
    }
}