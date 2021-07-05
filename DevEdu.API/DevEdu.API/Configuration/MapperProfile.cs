using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Configuration
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMappingToDto();
            CreateMappingFromDto();
        }

        private void CreateMappingToDto()
        {
            CreateMap<StudentAnswerOnTaskInputModel, StudentAnswerOnTaskDto>();
        }

        private void CreateMappingFromDto()
        {

        }
    }
}
