using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        Task<CommentDto> AddCommentToStudentAnswerAsync(int studentHomeworkId, CommentDto dto, UserIdentityInfo userInfo);
        Task DeleteCommentAsync(int commentId, UserIdentityInfo userInfo);
        Task<CommentDto> GetCommentAsync(int commentId, UserIdentityInfo userInfo);
        Task<CommentDto> UpdateCommentAsync(int commentId, CommentDto dto, UserIdentityInfo userInfo);
    }
}