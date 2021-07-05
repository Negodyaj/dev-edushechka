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
            CreateMap<MaterialInputModel, MaterialDto>();
        }

        private void CreateMappingFromDto()
        {
            CreateMap<MaterialDto, MaterialInputModel>();
        }
    }
}
