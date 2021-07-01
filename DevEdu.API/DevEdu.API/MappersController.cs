using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}