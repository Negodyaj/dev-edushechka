using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto AddCommentToLesson(int lessonId, CommentDto dto);
        CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto);
        CommentDto GetComment(int id);
        void DeleteComment(int id);
        CommentDto UpdateComment(int id, CommentDto dto);
    }
}