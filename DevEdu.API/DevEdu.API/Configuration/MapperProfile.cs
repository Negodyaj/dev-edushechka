using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
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
            CreateMap<CourseTopicInputModel, CourseTopicDto>();
            CreateMap<CommentAddInputModel, CommentDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDto { Id = src.UserId }));
            CreateMap<CommentUpdateInputModel, CommentDto>();
            CreateMap<GroupInputModel, GroupDto>();
            CreateMap<MaterialInputModel, MaterialDto>();
            CreateMap<NotificationAddInputModel, NotificationDto>();
            CreateMap<NotificationUpdateInputModel, NotificationDto>();
            CreateMap<StudentAnswerOnTaskInputModel, StudentAnswerOnTaskDto>();
            CreateMap<LessonInputModel, LessonDto>();
            CreateMap<TagInputModel, TagDto>();
            CreateMap<TaskInputModel, TaskDto>();
            CreateMap<TopicInputModel, TopicDto>();
            CreateMap<UserInsertInputModel, UserDto>();
            CreateMap<UserUpdateInputModel, UserDto>();
        }

        private void CreateMappingFromDto()
        {
            CreateMap<CourseDto, CourseInfoOutputModel>();
        }
    }
}