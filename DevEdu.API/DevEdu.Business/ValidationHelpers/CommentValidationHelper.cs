using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
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

        public void CheckCommentExistence(int commentId)
        {
            var comment = _commentRepository.GetComment(commentId);
            if (comment == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(comment), commentId));
        }
    }
}