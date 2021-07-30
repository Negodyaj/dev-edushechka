using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class RatingValidationHelper : IRatingValidationHelper
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingValidationHelper(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public void CheckRaitingExistence(int ratingId)
        {
            var rating = _ratingRepository.SelectStudentRatingById(ratingId);
            if (rating == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(rating), ratingId));
        }
    }
}