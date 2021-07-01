using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;

namespace DevEdu.API
{
    public class MappersController
    {
        public CommentDto MapCommentModelToCommentDto(CommentAddtInputModel comment)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<CommentAddtInputModel, CommentDto>()));
            return mapper.Map<CommentDto>(comment);
        }

        public CommentDto MapCommentModelToCommentDto(CommentUpdatetInputModel comment)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<CommentUpdatetInputModel, CommentDto>()));
            return mapper.Map<CommentDto>(comment);
        }
        public CourseDto MapCourseModelToCourseDto(CourseInputModel comment)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<CourseInputModel, CourseDto>()));
            return mapper.Map<CourseDto>(comment);
        }
    }
}