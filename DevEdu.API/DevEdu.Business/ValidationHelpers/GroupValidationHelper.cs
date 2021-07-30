using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class GroupValidationHelper : IGroupValidationHelper
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public GroupValidationHelper(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public void CheckGroupExistence(int groupId)
        {
            //var group = _groupRepository.GetGroup(groupId);
            //if (group == default)
            //    throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), groupId));
        }
    }
}