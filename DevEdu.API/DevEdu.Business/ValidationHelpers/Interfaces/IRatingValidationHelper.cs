using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IRatingValidationHelper
    {
        Task<StudentRatingDto> CheckRaitingExistenceAndReturnDtoAsync(int ratingId);
    }
}