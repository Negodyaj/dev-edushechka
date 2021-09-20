using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        Task<CommentDto> AddCommentToLessonAsync(int lessonId, CommentDto dto, UserIdentityInfo userIdentityInfo);
        Task<CommentDto> AddCommentToStudentHomeworkAsync(int studentHomeworkId, CommentDto dto, UserIdentityInfo userIdentityInfo);
        Task<CommentDto> GetCommentAsync(int id, UserIdentityInfo userIdentityInfo);
        Task DeleteCommentAsync(int id, UserIdentityInfo userIdentityInfo);
        Task<CommentDto> UpdateCommentAsync(int id, CommentDto dto, UserIdentityInfo userInfo);
    }
}