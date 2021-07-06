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
            CreateMap<TaskInputModel, TaskDto>();
            CreateMap<CourseInputModel, CourseDto>().ReverseMap();
            CreateMap<GroupInputModel, GroupDto>().ReverseMap();
            CreateMap<StudentAnswerOnTaskInputModel, StudentAnswerOnTaskDto>();
            CreateMap<FeedbackInputModel, StudentLessonDto>();
            CreateMap<AbsenceReasonInputModel, StudentLessonDto>();
            CreateMap<AttendanceInputModel, StudentLessonDto>();
        }

        private void CreateMappingFromDto()
        {

        }
    }
}