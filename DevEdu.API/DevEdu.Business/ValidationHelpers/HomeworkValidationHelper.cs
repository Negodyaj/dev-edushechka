using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class HomeworkValidationHelper : IHomeworkValidationHelper
    {
        private readonly IHomeworkRepository _commentRepository;

        public HomeworkValidationHelper(IHomeworkRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public void CheckCommentExistence(int homeworkId)
        {
            var homework = _commentRepository.GetHomework(homeworkId);
            if (homework == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homeworkId));
        }
    }
}