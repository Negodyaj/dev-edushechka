using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICommentValidationHelper
    {
        CommentDto GetCommentByIdAndThrowIfNotFound(int commentId);
        void UserComplianceCheck(CommentDto dto, int userId);
    }
}