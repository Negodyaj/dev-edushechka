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

        public int AddGroupMaterialReference(int groupId, int materialId) => _groupRepository.AddGroupMaterialReference(groupId, materialId);

        public int RemoveGroupMaterialReference(int groupId, int materialId) => _groupRepository.RemoveGroupMaterialReference(groupId, materialId);

        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupRepository.AddUserToGroup(groupId, userId, roleId);

        public void DeleteUserFromGroup(int groupId, int userId) => _groupRepository.DeleteUserFromGroup(userId, groupId);
    }
}