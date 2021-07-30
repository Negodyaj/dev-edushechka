using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class GroupValidationHelper : IGroupValidationHelper
    {
        private readonly IGroupRepository _groupRepository;

        public GroupValidationHelper(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public void CheckGroupExistence(int groupId)
        {
            var group = _groupRepository.GetGroup(groupId);
            if (group == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), groupId));
        }

        //public void CheckProvidedGroupsAreUnique(List<int> groups)
        //{
        //    if (!(groups.Distinct().Count() == groups.Count()))
        //        throw new ValidationException(ServiceMessages.DuplicateCoursesValuesProvided);
        //}
    }
}