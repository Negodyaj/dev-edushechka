using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class RatingValidationHelper : IRatingValidationHelper
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingValidationHelper(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<StudentRatingDto> CheckRaitingExistenceAndReturnDtoAsync(int ratingId)
        {
            var rating = await _ratingRepository.SelectStudentRatingByIdAsync(ratingId);
            if (rating == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(rating), ratingId));
            
            return rating;
        }
    }
}