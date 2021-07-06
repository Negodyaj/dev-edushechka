using DevEdu.DAL.Models;

namespace DevEdu.Business.Servicies
{
    public interface ICommentService
    {
        CommentDto GetComment(int id);
        void UpdateComment(int id, CommentDto dto);
    }
}