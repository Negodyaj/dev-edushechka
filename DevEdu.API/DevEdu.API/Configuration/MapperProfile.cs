using System;
using System.Globalization;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Models;

namespace DevEdu.API.Configuration
{
    public class MapperProfile : Profile
    {
        CultureInfo cultureRu = CultureInfo.CreateSpecificCulture("ru-RU");
        public MapperProfile()
        {
            CreateMappingToDto();
            CreateMappingFromDto();
        }

        private void CreateMappingToDto()
        { 
            CreateMap<AbsenceReasonInputModel, StudentLessonDto>();
            CreateMap<AttendanceInputModel, StudentLessonDto>();
            CreateMap<CourseInputModel, CourseDto>();
            CreateMap<CourseTopicInputModel, CourseTopicDto>();
            CreateMap<CommentAddInputModel, CommentDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDto { Id = src.UserId }));
            CreateMap<CommentUpdateInputModel, CommentDto>();
            CreateMap<FeedbackInputModel, StudentLessonDto>();
            CreateMap<GroupInputModel, GroupDto>();
            CreateMap<MaterialInputModel, MaterialDto>();
            CreateMap<NotificationAddInputModel, NotificationDto>();
            CreateMap<NotificationUpdateInputModel, NotificationDto>();
            CreateMap<StudentAnswerOnTaskInputModel, StudentAnswerOnTaskDto>();
            CreateMap<LessonInputModel, LessonDto>();
            CreateMap<TagInputModel, TagDto>();
            CreateMap<TaskInputModel, TaskDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.Parse(src.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateTime.Parse(src.EndDate)));
            CreateMap<TopicInputModel, TopicDto>();
            CreateMap<UserInsertInputModel, UserDto>();
            CreateMap<UserUpdateInputModel, UserDto>();
        }

        private void CreateMappingFromDto()
        {
            CreateMap<CourseDto, CourseInfoOutputModel>();
            CreateMap<TaskDto, TaskInfoOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("d", cultureRu)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("d", cultureRu))); ;
            CreateMap<TagDto, TagInfoOutputModel>();
            CreateMap<StudentAnswerOnTaskDto, StudentAnswerOnTaskInfoOutputModel>();
        }
    }
}