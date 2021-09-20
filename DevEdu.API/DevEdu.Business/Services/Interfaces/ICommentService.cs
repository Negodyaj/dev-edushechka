using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto AddCommentToLesson(int lessonId, CommentDto dto, UserIdentityInfo userIdentityInfo);
        Task<CommentDto> AddCommentToStudentAnswer(int studentHomeworkId, CommentDto dto, UserIdentityInfo userIdentityInfo);
        CommentDto GetComment(int id, UserIdentityInfo userIdentityInfo);
        void DeleteComment(int id, UserIdentityInfo userIdentityInfo);
        CommentDto UpdateComment(int id, CommentDto dto, UserIdentityInfo userInfo);
    }
}