using System.Threading.Tasks;
using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICommentValidationHelper
    {
        Task<CommentDto> GetCommentByIdAndThrowIfNotFoundAsync(int commentId);
        Task UserComplianceCheckAsync(CommentDto dto, int userId);
    }
}