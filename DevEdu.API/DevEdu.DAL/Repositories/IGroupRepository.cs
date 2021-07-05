namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        void AddGroupMaterialReference(int materialId, int groupId);
        void RemoveGroupMaterialReference(int materialId, int groupId);
    }
}