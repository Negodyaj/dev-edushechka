using System.Threading.Tasks;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class CommentValidationHelper : ICommentValidationHelper
    {
        private readonly ICommentRepository _commentRepository;

        public CommentValidationHelper(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDto> GetCommentByIdAndThrowIfNotFoundAsync(int commentId)
        {
            var comment = await _commentRepository.GetCommentAsync(commentId);
            if (comment == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(comment), commentId));
            return comment;
        }

        public async Task UserComplianceCheckAsync(CommentDto dto, int userId)
        {
            if (dto.User.Id != userId)
                 throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }
    }
}