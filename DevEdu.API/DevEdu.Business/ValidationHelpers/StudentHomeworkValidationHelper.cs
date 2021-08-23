using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class StudentHomeworkValidationHelper : IStudentHomeworkValidationHelper
    {
        private readonly IStudentHomeworkRepository _studentHomeworkRepository;
        private readonly IGroupRepository _groupRepository;

        public StudentHomeworkValidationHelper
        (
            IStudentHomeworkRepository studentHomeworkRepository,
            IGroupRepository groupRepository
        )
        {
            _studentHomeworkRepository = studentHomeworkRepository;
            _groupRepository = groupRepository;
        }

        public StudentHomeworkDto GetStudentHomeworkByIdAndThrowIfNotFound(int id)
        {
            var studentHomework = _studentHomeworkRepository.GetStudentHomeworkById(id);
            if (studentHomework == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), id));
            return studentHomework;
        }

        public void CheckUserBelongsToHomework(int groupId, int userId)
        {
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            var group = _groupRepository.GetGroup(groupId);
            var result = groupsByUser.FirstOrDefault(gu => gu.Id == @group.Id);
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserInGroupNotFoundMessage, userId, groupId));
        }

        public void CheckUserInStudentHomeworkAccess(int studentId, int userId)
        {
            var groupsByStudent = _groupRepository.GetGroupsByUserId(studentId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            var result = groupsByUser.FirstOrDefault(gu => groupsByStudent.Any(gs => gs.Id == gu.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }

        public void CheckUserComplianceToStudentHomework(int studentId, int userId)
        {
            if (studentId != userId)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }
    }
}