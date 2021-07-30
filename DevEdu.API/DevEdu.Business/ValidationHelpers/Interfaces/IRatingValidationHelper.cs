using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IRatingValidationHelper
    {
        void CheckRaitingExistence(int raitingId);
    }
}