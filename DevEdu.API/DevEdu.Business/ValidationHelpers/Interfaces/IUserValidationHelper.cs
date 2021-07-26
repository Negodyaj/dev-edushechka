namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        void CheckUserExistence(int userId);
        void CheckUserIdAndRoleIdDoesNotLessThanMinimum(int userId, int roleId);
        void ChekRoleExistence(int roleId);
        void ChekIdDoesNotLessThenMinimum(int id);
    }
}