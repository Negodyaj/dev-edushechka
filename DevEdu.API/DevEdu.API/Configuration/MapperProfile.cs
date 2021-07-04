using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;

namespace DevEdu.API.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            MapModelToDto();
            MapDtoToModel();
        }

        private void MapModelToDto()
        {
            CreateMap<UserInsertInputModel, UserDto>();
            CreateMap<UserUpdateInputModel, UserDto>();
        }
        private void MapDtoToModel()
        {

        }
    }
}
