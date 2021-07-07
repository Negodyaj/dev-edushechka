using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Servicies
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;

        public GroupService(IGroupRepository repository)
        {
            _repository = repository;
        }

        public void AddUserToGroup(int groupId, int userId, int roleId) => _repository.AddUserToGroup(groupId, userId, roleId);

        public void DeleteUserFromGroup(int groupId, int userId) => _repository.DeleteUserFromGroup(userId, groupId);
    }
}
