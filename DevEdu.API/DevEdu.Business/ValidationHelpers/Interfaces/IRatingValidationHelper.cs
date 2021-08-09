using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IRatingValidationHelper
    {
        public StudentRatingDto CheckRaitingExistenceAndReturnDto(int ratingId);
    }
}