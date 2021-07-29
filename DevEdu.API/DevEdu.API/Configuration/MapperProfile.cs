using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.API.Models.OutputModels.Payment;
using DevEdu.API.Models.OutputModels.Lesson;
using DevEdu.DAL.Models;
using System;
using System.Globalization;

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
            CreateMap<GroupInputModel, GroupDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.StartDate, _dateFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.GroupStatus, opt => opt.MapFrom(src => src.GroupStatusId != null ? src.GroupStatusId : null));
            CreateMap<GroupTaskInputModel, GroupTaskDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.StartDate, _dateFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.EndDate, _dateFormat, CultureInfo.InvariantCulture)));
            CreateMap<MaterialInputModel, MaterialDto>();
            CreateMap<NotificationAddInputModel, NotificationDto>() 
                .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.GroupId != null ? new GroupDto { Id = (int)src.GroupId } : null))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserId != null ? new UserDto { Id = (int)src.UserId } : null))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleId != null ? src.RoleId : null));
            CreateMap<NotificationUpdateInputModel, NotificationDto>();
            CreateMap<StudentAnswerOnTaskInputModel, StudentAnswerOnTaskDto>();
            CreateMap<LessonInputModel, LessonDto>()
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => new UserDto { Id = src.TeacherId }))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.ParseExact(src.Date, _dateFormat, CultureInfo.InvariantCulture)));
            CreateMap<LessonUpdateInputModel, LessonDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.ParseExact(src.Date, _dateFormat, CultureInfo.InvariantCulture)));
            CreateMap<TagInputModel, TagDto>();
            CreateMap<TaskInputModel, TaskDto>();
            CreateMap<TopicInputModel, TopicDto>();
            CreateMap<UserInsertInputModel, UserDto>();
            CreateMap<UserUpdateInputModel, UserDto>();
            CreateMap<AbsenceReasonInputModel, StudentLessonDto>();
            CreateMap<AttendanceInputModel, StudentLessonDto>();
            CreateMap<FeedbackInputModel, StudentLessonDto>();
            CreateMap<PaymentInputModel, PaymentDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDto { Id = src.UserId }))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.ParseExact(src.Date, _dateFormat, CultureInfo.InvariantCulture)));
            CreateMap<CourseTopicUpdateInputModel, CourseTopicDto>()
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => new TopicDto { Id = src.TopicId }));
            CreateMap<StudentRatingInputModel, StudentRatingDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDto { Id = src.UserId }))
                .ForMember(dest => dest.Group, opt => opt.MapFrom(src => new GroupDto { Id = src.GroupId }))
                .ForMember(dest => dest.RatingType, opt => opt.MapFrom(src => new RatingTypeDto { Id = src.RatingTypeId }));
            CreateMap<UserSignInputModel, UserDto>();
            CreateMap<PaymentUpdateInputModel, PaymentDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.ParseExact(src.Date, _dateFormat, CultureInfo.InvariantCulture)));
        }

        private void CreateMappingFromDto()
        {
            CreateMap<CourseDto, CourseInfoOutputModel>();
            CreateMap<GroupDto, GroupOutputBaseModel>();
            CreateMap<GroupDto, GroupOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)));
            CreateMap<GroupDto, GroupFullOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)));
            CreateMap<GroupDto, GroupInfoOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)));
            CreateMap<TopicDto, TopicOutputModel>();
            CreateMap<CommentDto, CommentInfoOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<CourseTopicDto, CourseTopicOutputModel>();
            CreateMap<CourseDto, CourseInfoBaseOutputModel>();
            CreateMap<MaterialDto, MaterialInfoOutputModel>();
            CreateMap<MaterialDto, MaterialInfoFullOutputModel>();
            CreateMap<MaterialDto, MaterialInfoWithGroupsOutputModel>();
            CreateMap<MaterialDto, MaterialInfoWithCoursesOutputModel>();
            CreateMap<UserDto, UserInfoOutPutModel>();
            CreateMap<UserDto, UserFullInfoOutPutModel>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate.ToString(_dateFormat)))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToString(_dateFormat)))
                .ForMember(dest => dest.ExileDate, opt => opt.MapFrom(src => src.ExileDate.ToString(_dateFormat)));
            CreateMap<UserDto, UserUpdateInfoOutPutModel>();
            CreateMap<UserDto, UserInfoOutPutModel>();
            CreateMap<UserDto, UserInfoShortOutputModel>();
            CreateMap<CourseDto, CourseInfoShortOutputModel>();
            CreateMap<TaskDto, TaskInfoOutputModel>();
            CreateMap<TaskDto, TaskInfoWithCoursesOutputModel>();
            CreateMap<TaskDto, TaskInfoWithCoursesAndAnswersOutputModel>();
            CreateMap<TaskDto, TaskInfoWithAnswersOutputModel>();
            CreateMap<TagDto, TagOutputModel>();
            CreateMap<StudentAnswerOnTaskForTaskDto, StudentAnswerOnTaskInfoOutputModel>();
            CreateMap<StudentAnswerOnTaskDto, StudentAnswerOnTaskInfoOutputModel>();
            CreateMap<StudentAnswerOnTaskDto, StudentAnswerOnTaskFullOutputModel>()
                .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate != null ? ((DateTime)src.CompletedDate).ToString(_dateFormat) : null));
            CreateMap<StudentAnswerOnTaskDto, StudentAnswerOnTaskOutputModel>()
                .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate != null ? ((DateTime)src.CompletedDate).ToString(_dateFormat) : null));
            CreateMap<GroupDto, GroupInfoOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)));
            CreateMap<LessonDto, LessonInfoOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<LessonDto, LessonInfoWithCourseOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<LessonDto, LessonInfoWithCommentsOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<LessonDto, LessonInfoWithStudentsAndCommentsOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<StudentLessonDto, StudentLessonOutputModel>();
            CreateMap<StudentRatingDto, StudentRatingOutputModel>();
            CreateMap<RatingTypeDto, RatingTypeOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoWithGroupOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoWithTaskOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoFullOutputModel>();
            CreateMap<GroupTaskDto, GroupTaskInfoOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString(_dateFormat)));
            CreateMap<GroupDto, GroupOutputMiniModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString(_dateFormat)));
            CreateMap<TaskDto, TaskInfoOutputMiniModel>();
            CreateMap<NotificationDto, NotificationInfoOutputModel>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Role != null ? src.Role : null))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)));
            CreateMap<PaymentDto, PaymentOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString(_dateFormat)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src =>
               new UserInfoShortOutputModel { Id = src.User.Id,
                   FirstName = src.User.FirstName,
                   LastName = src.User.LastName,
                   Email = src.User.Email,
                   Photo = src.User.Photo }));
            CreateMap<StudentLessonDto, FeedbackOutputModel>();
            CreateMap<StudentLessonDto, StudentLessonShortOutputModel>();
            CreateMap<StudentRatingDto, StudentRatingOutputModel>();
            CreateMap<RatingTypeDto, RatingTypeOutputModel>();

        }
    }
}