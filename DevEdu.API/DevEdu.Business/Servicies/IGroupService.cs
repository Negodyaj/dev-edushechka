namespace DevEdu.Business.Servicies
{
    public interface IGroupService
    {
        string AddGroupMaterialReference(int materialId, int groupId);
        string RemoveGroupMaterialReference(int materialId, int groupId);
    }
}