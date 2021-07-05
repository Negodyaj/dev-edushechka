namespace DevEdu.DAL.Repositories
{
    public interface IUserRoleRepository
    {
        int AddUserRole(int userId, int roleId);
        void DeleteUserRole(int userId, int roleId);
    }
}