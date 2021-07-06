using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Servicies
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public string AddGroupMaterialReference(int materialId, int groupId)
        {
            _groupRepository.AddGroupMaterialReference(materialId, groupId);
            return $"Material №{materialId} add to group {groupId}";
        }

        public string RemoveGroupMaterialReference(int materialId, int groupId)
        {
            _groupRepository.RemoveGroupMaterialReference(materialId, groupId);
            return $"Material №{materialId} remove from group {groupId}";
        }
    }
}