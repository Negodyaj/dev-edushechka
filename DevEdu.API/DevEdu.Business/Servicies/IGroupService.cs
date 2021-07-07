namespace DevEdu.Business.Servicies
{
    public interface IGroupService
    {
        void AddUserToGroup(int groupId, int userId, int roleId);
        void DeleteUserFromGroup(int groupId, int userId);
    }
}