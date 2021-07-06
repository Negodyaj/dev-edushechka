namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        void AddGroupLesson(int groupId, int lessonId);
        void RemoveGroupLesson(int groupId, int lessonId);
        void AddGroupMaterialReference(int materialId, int groupId);
        void RemoveGroupMaterialReference(int materialId, int groupId);
    }
}