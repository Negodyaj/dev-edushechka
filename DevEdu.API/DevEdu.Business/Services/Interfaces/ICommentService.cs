using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto AddCommentToLesson(int lessonId, CommentDto dto, UserIdentityInfo userIdentityInfo);
        CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto, UserIdentityInfo userIdentityInfo);
        CommentDto GetComment(int id, UserIdentityInfo userIdentityInfo);
        void DeleteComment(int id, UserIdentityInfo userIdentityInfo);
        CommentDto UpdateComment(int id, CommentDto dto, UserIdentityInfo userInfo);
    }
}