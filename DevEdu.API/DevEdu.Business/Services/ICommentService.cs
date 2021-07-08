using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto GetComment(int id);
        void UpdateComment(int id, CommentDto dto);
    }
}