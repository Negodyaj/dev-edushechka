using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
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

        public StudentHomeworkDto GetStudentAnswerByIdAndThrowIfNotFound(int id)
        {
            var studentAnswerOnTask = _studentHomeworkRepository.GetStudentAnswerOnTaskById(id);
            if (studentAnswerOnTask == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTask), id));
            return studentAnswerOnTask;
        }

        public void CheckUserInStudentAnswerAccess(int studentId, int userId)
        {
            var groupsByStudent = _groupRepository.GetGroupsByUserId(studentId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            var result = groupsByUser.FirstOrDefault(gu => groupsByStudent.Any(gs => gs.Id == gu.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }

        public void CheckUserComplianceToStudentAnswer(StudentHomeworkDto dto, int userId)
        {
            if (dto.User.Id != userId)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }

        public List<StudentHomeworkDto> GetStudentAnswerOnTaskAllowedToUser(int taskId, int userId)
        {
            var groupsByTask = _groupRepository.GetGroupsByTaskId(taskId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));

            if (result == default)
                return null;

            return _studentHomeworkRepository.GetAllStudentAnswersOnTask(taskId);
        }

        public void CheckUserAccessToStudentAnswerByUserId(UserIdentityInfo userInfo, StudentHomeworkDto studentAnswerDto)
        {
            var userId = userInfo.UserId;

            if (userInfo.IsAdmin())
            {
                return;
            }
            CheckUserComplianceToStudentAnswer(studentAnswerDto, userId);
        }
    }
}