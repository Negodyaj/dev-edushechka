using AutoMapper;
using DevEdu.API.Models.InputModels;

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
            CreateMap<TaskInputModel, TopicDto>();
         
        }

        private void CreateMappingFromDto()
        {

        }
    }
}