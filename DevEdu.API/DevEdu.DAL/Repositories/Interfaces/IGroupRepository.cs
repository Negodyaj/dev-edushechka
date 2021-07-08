namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        int AddUserToGroup(int groupId, int userId, int roleId);
        int DeleteUserFromGroup(int userId, int groupId);
        void AddGroupLesson(int groupId, int lessonId);
        void RemoveGroupLesson(int groupId, int lessonId);
        void AddGroupMaterialReference(int materialId, int groupId);
        void RemoveGroupMaterialReference(int materialId, int groupId);
    }
}