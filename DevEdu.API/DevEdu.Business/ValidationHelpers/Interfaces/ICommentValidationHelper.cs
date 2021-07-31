using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICommentValidationHelper
    {
        CommentDto CheckCommentExistence(int commentId);
        void CheckUserForCommentAccess(CommentDto dto, int userId);
        void CheckUser(CommentDto dto, int userId);
    }
}