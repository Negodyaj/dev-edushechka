namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        int AddGroupLesson(int groupId, int materialId);
        int RemoveGroupLesson(int groupId, int materialId);
        void AddUserToGroup(int groupId, int userId, int roleId);
        void DeleteUserFromGroup(int groupId, int userId);
    }
}