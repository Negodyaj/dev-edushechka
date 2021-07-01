using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;

namespace DevEdu.API
{
    public class MappersController
    {
        public CommentDto MapCommentModelToDto(CommentAddtInputModel comment)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<CommentAddtInputModel, CommentDto>()));
            return mapper.Map<CommentDto>(comment);
        }

        public CommentDto MapCommentModelToDto(CommentUpdatetInputModel comment)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<CommentUpdatetInputModel, CommentDto>()));
            return mapper.Map<CommentDto>(comment);
        }
    }
}