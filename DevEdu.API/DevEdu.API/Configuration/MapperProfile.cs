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
        private const string _dateFormat = "dd.MM.yyyy";
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
            CreateMap<LessonInputModel, LessonDto>()
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => new UserDto { Id = src.TeacherId }));
            CreateMap<LessonUpdateInputModel, LessonDto>();
            CreateMap<TagInputModel, TagDto>();
            CreateMap<TaskInputModel, TaskDto>();
            CreateMap<TopicInputModel, TopicDto>();
            CreateMap<UserInsertInputModel, UserDto>();
            CreateMap<UserUpdateInputModel, UserDto>();
            CreateMap<AbsenceReasonInputModel, StudentLessonDto>();
            CreateMap<AttendanceInputModel, StudentLessonDto>();
            CreateMap<FeedbackInputModel, StudentLessonDto>();
            CreateMap<StudentRaitingInputModel, StudentRaitingDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDto { Id = src.UserId }))
                .ForMember(dest => dest.Group, opt => opt.MapFrom(src => new GroupDto { Id = src.GroupId }))
                .ForMember(dest => dest.RaitingType, opt => opt.MapFrom(src => new RaitingTypeDto { Id = src.RaitingTypeId }));
        }

        private void CreateMappingFromDto()
        {
            CreateMap<CourseDto, CourseInfoOutputModel>();
            CreateMap<TopicDto, TopicOutputModel>();
            CreateMap<CommentDto, CommentInfoOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<CourseTopicDto, CourseTopicOutputModel>();
            CreateMap<UserDto, UserInfoOutputModel>();
            CreateMap<UserDto, UserInfoShortOutputModel>(); 
            CreateMap<CourseDto, CourseInfoShortOutputModel>();
            CreateMap<TaskDto, TaskInfoOutputModel>();
            CreateMap<TaskDto, TaskInfoWithCoursesOutputModel>();
            CreateMap<TaskDto, TaskInfoWithCoursesAndAnswersOutputModel>();
            CreateMap<TaskDto, TaskInfoWithAnswersOutputModel>();
            CreateMap<TagDto, TagInfoOutputModel>();
            CreateMap<StudentAnswerOnTaskForTaskDto, StudentAnswerOnTaskInfoOutputModel>();
            CreateMap<StudentAnswerOnTaskDto, StudentAnswerOnTaskInfoOutputModel>();
            CreateMap<TagDto, TagOutputModel>();
            CreateMap<StudentRaitingDto, StudentRaitingOutputModel>();
            CreateMap<RaitingTypeDto, RaitingTypeOutputModel>();
            CreateMap<UserDto, UserInfoOutputModel>();
        }
    }
}