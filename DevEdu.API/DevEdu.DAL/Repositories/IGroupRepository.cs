namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        void AddGroupLesson(int groupId, int lessonId);
        void RemoveGroupLesson(int groupId, int lessonId);
        int AddGroupMaterialReference(int materialId, int groupId);
        int RemoveGroupMaterialReference(int materialId, int groupId);
    }
}