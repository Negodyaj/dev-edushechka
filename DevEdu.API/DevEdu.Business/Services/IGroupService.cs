namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        int AddGroupMaterialReference(int materialId, int groupId);
        int RemoveGroupMaterialReference(int materialId, int groupId);
    }
}