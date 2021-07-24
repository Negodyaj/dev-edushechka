namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        void CheckUserExistence(int userId);
        void ChekRoleExistence(int roleId);
    }
}