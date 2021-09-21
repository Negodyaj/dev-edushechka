using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICommentValidationHelper
    {
        Task<CommentDto> GetCommentByIdAndThrowIfNotFoundAsync(int commentId);
        void UserComplianceCheck(CommentDto dto, int userId);
    }
}