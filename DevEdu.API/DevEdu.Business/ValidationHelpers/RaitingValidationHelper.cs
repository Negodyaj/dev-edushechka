using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class RaitingValidationHelper : IRaitingValidationHelper
    {
        private readonly IRaitingRepository _raitingRepository;

        public RaitingValidationHelper(IRaitingRepository raitingRepository)
        {
            _raitingRepository = raitingRepository;
        }

        public void CheckRaitingExistence(int raitingId)
        {
            var raiting = _raitingRepository.SelectStudentRaitingById(raitingId);
            if (raiting == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(raiting), raitingId));
        }
    }
}