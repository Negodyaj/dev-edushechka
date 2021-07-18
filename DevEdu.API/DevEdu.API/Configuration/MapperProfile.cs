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
        private readonly string _dateFormat = "dd.MM.yyyy";
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
            CreateMap<GroupTaskInputModel, GroupTaskDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.Parse(src.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateTime.Parse(src.EndDate)));
            CreateMap<MaterialInputModel, MaterialDto>();
            CreateMap<NotificationAddInputModel, NotificationDto>();
            CreateMap<NotificationUpdateInputModel, NotificationDto>();
            CreateMap<StudentAnswerOnTaskInputModel, StudentAnswerOnTaskDto>();
            CreateMap<LessonInputModel, LessonDto>()
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => new UserDto { Id = src.TeacherId}));
            CreateMap<LessonUpdateInputModel, LessonDto>();
            CreateMap<TagInputModel, TagDto>();
            CreateMap<TaskInputModel, TaskDto>();
            CreateMap<TopicInputModel, TopicDto>();
            CreateMap<UserInsertInputModel, UserDto>();
            CreateMap<UserUpdateInputModel, UserDto>();
        }

        private void CreateMappingFromDto()
        {
            CreateMap<CourseDto, CourseInfoOutputModel>();
            CreateMap<TopicDto, TopicOutputModel>();
            CreateMap<CommentDto, CommentInfoOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<CourseTopicDto, CourseTopicOutputModel>();
            CreateMap<UserDto, UserInfoOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoWithGroupOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoWithTaskOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoFullOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString(_dateFormat)));
            CreateMap<GroupDto, GroupOutputMiniModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)));
            CreateMap<TaskDto, TaskInfoOutputMiniModel>();
        }
    }
}