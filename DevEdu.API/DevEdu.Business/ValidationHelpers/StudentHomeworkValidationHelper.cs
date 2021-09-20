using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<StudentHomeworkDto> GetStudentHomeworkByIdAndThrowIfNotFound(int id)
        {
            var studentHomework = await _studentHomeworkRepository.GetStudentHomeworkByIdAsync(id);
            if (studentHomework == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), id));
            return studentHomework;
        }

        public async Task CheckUserBelongsToHomeworkAsync(int groupId, int userId)
        {
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);
            var group = Task.Run(async () => await _groupRepository.GetGroup(groupId)).Result;
            var result = groupsByUser.FirstOrDefault(gu => gu.Id == @group.Id);
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserInGroupNotFoundMessage, userId, groupId));
        }

        public async Task CheckUserInStudentHomeworkAccessAsync(int studentId, int userId)
        {
            var groupsByStudent = await _groupRepository.GetGroupsByUserIdAsync(studentId);
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);
            var result = groupsByUser.FirstOrDefault(gu => groupsByStudent.Any(gs => gs.Id == gu.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }

        public async Task CheckUserComplianceToStudentHomeworkAsync(int studentId, int userId)
        {
            if (studentId != userId)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }
    }
}