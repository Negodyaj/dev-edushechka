using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto AddCommentToLesson(int lessonId, CommentDto dto, int userId, List<Role> roles);
        CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto, int userId, List<Role> roles);
        CommentDto GetComment(int id, int userId, List<Role> roles);
        void DeleteComment(int id, int userId, List<Role> roles);
        CommentDto UpdateComment(int id, CommentDto dto, int userId, List<Role> roles);
    }
}