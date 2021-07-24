namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        void CheckUserExistence(int userId);
        void CheckUserIdAndRoleIdDoesNotLessThanZero(int userId, int roleId);
        void ChekRoleExistence(int roleId);
        void ChekUserIdDoesNotLessThenZero(int id);
    }
}