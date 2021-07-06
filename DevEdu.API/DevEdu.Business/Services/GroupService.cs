using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public int AddGroupMaterialReference(int materialId, int groupId)
        {
            return _groupRepository.AddGroupMaterialReference(materialId, groupId);
        }

        public int RemoveGroupMaterialReference(int materialId, int groupId)
        {
            return _groupRepository.RemoveGroupMaterialReference(materialId, groupId);
        }
    }
}