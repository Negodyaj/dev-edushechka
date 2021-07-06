namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        void AddUser_Group(int groupId, int userId, int roleId);
        void DeleteUserFromGroup(int userId, int groupId);
        void AddGroupMaterialReference(int materialId, int groupId);
        void RemoveGroupMaterialReference(int materialId, int groupId);
    }
}