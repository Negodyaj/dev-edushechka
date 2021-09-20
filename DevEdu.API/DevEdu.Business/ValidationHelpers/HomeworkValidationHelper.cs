using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class HomeworkValidationHelper : IHomeworkValidationHelper
    {
        private readonly IHomeworkRepository _homeworkRepository;

        public HomeworkValidationHelper(IHomeworkRepository homeworkRepository)
        {
            _homeworkRepository = homeworkRepository;
        }

        public async Task<HomeworkDto> GetHomeworkByIdAndThrowIfNotFoundAsync(int homeworkId)
        {
            var homework = await _homeworkRepository.GetHomeworkAsync(homeworkId);
            if (homework == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homeworkId));
            
            return homework;
        }
    }
}