using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        UserDto GetUserByIdAndThrowIfNotFound(int userId);
    }
}