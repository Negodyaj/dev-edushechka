using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public GroupDto AddGroup(GroupDto groupDto) => _groupRepository.AddGroup(groupDto);

        public void DeleteGroup(int id) => _groupRepository.DeleteGroup(id);

        public GroupDto GetGroup(int id) => _groupRepository.GetGroup(id);

        public List<GroupDto> GetGroups() => _groupRepository.GetGroups();

        public GroupDto UpdateGroup(int id, GroupDto groupDto) => _groupRepository.UpdateGroup(id, groupDto);

        public int AddGroupMaterialReference(int groupId, int materialId) => _groupRepository.AddGroupMaterialReference(groupId, materialId);

        public int RemoveGroupMaterialReference(int groupId, int materialId) => _groupRepository.RemoveGroupMaterialReference(groupId, materialId);

        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupRepository.AddUserToGroup(groupId, userId, roleId);

        public void DeleteUserFromGroup(int groupId, int userId) => _groupRepository.DeleteUserFromGroup(userId, groupId);
    }
}