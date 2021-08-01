using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto AddCommentToLesson(int lessonId, CommentDto dto, UserToken userToken);
        CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto, UserToken userToken);
        CommentDto GetComment(int id, UserToken userToken);
        void DeleteComment(int id, UserToken userToken);
        CommentDto UpdateComment(int id, CommentDto dto, UserToken userToken);
    }
}