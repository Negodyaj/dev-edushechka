using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            CreateMap<CourseTopicInputModel, CourseTopicDto>();
        }
        private void CreateMappingFromDto()
        {
            CreateMap<CourseDto, CourseInputModel>();
            CreateMap<CourseTopicDto, CourseTopicInputModel>();
        }
    }
}
