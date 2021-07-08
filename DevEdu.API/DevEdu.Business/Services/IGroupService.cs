namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
    }
}