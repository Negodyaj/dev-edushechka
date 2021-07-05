namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        void AddGroupMaterialReference(int materialId, int groupId);
        void DeleteGroupMaterialReference(int materialId, int groupId);
    }
}