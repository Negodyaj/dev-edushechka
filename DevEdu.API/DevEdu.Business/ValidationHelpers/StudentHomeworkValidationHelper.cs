using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
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

        public async Task<StudentHomeworkDto> GetStudentHomeworkByIdAndThrowIfNotFoundAsync(int id)
        {
            var studentHomework = await _studentHomeworkRepository.GetStudentHomeworkByIdAsync(id);
            if (studentHomework == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), id));

            return studentHomework;
        }

        public async Task CheckUserBelongsToHomeworkAsync(int groupId, int userId)
        {
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);
            var group = Task.Run(async () => await _groupRepository.GetGroupAsync(groupId)).Result;
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

        public void CheckUserCanChangeStatus(UserIdentityInfo userIdentityInfo, StudentHomeworkDto studentHomeworkDto,
                                            StudentHomeworkStatus newStatus)
        {
            var currentStatus = studentHomeworkDto.StudentHomeworkStatus;
            
            if (currentStatus == StudentHomeworkStatus.Done || currentStatus == StudentHomeworkStatus.DoneWithLate)
                throw new ConflictExpection(ServiceMessages.HomeworkStatusCantBeChanged);

            if (currentStatus == newStatus)
                throw new ConflictExpection(ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);

            if (currentStatus == StudentHomeworkStatus.NotDone && newStatus != StudentHomeworkStatus.OnCheck)
                throw new ConflictExpection(ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);

            if ((currentStatus == StudentHomeworkStatus.OnCheck || currentStatus == StudentHomeworkStatus.OnCheckRepeat)
                && newStatus != StudentHomeworkStatus.ToFix
                && newStatus != StudentHomeworkStatus.Done
                && newStatus != StudentHomeworkStatus.DoneWithLate)
                throw new ConflictExpection(ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);

            if (currentStatus == StudentHomeworkStatus.ToFix
                && newStatus != StudentHomeworkStatus.OnCheckRepeat)
                throw new ConflictExpection(ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);

            if (userIdentityInfo.IsAdmin())
                return;

            if (userIdentityInfo.IsStudent())
            {
                if (currentStatus == StudentHomeworkStatus.OnCheck || currentStatus == StudentHomeworkStatus.OnCheckRepeat)
                    throw new AuthorizationException(ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
                return;
            }

            if (userIdentityInfo.IsTutor() || userIdentityInfo.IsTeacher())
            {
                if (currentStatus != StudentHomeworkStatus.OnCheck && currentStatus != StudentHomeworkStatus.OnCheckRepeat)
                    throw new AuthorizationException(ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
                return;
            }

            throw new AuthorizationException(ServiceMessages.HomeworkStatusCantBeChangedByThisUser);

        }
    }
}