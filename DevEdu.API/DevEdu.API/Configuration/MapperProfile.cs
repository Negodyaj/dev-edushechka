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
            CreateMap<CommentAddInputModel, CommentDto>();
            CreateMap<CommentUpdateInputModel, CommentDto>();
            CreateMap<MaterialInputModel, MaterialDto>();
            CreateMap<TaskInputModel, TaskDto>();
            CreateMap<TopicInputModel, TopicDto>();
            CreateMap<CourseInputModel, CourseDto>().ReverseMap();
            CreateMap<GroupInputModel, GroupDto>().ReverseMap();
            CreateMap<StudentAnswerOnTaskInputModel, StudentAnswerOnTaskDto>();
            CreateMap<TagInputModel, TagDto>();
            CreateMap<UserInsertInputModel, UserDto>();
            CreateMap<UserUpdateInputModel, UserDto>();
        }

        private void CreateMappingFromDto()
        {

        }
    }
}