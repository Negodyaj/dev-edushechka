using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentHomeworkValidationHelper
    {
        StudentHomeworkDto GetStudentAnswerByIdAndThrowIfNotFound(int id);
        void CheckUserInStudentAnswerAccess(int studentId, int userId);
        void CheckUserComplianceToStudentAnswer(StudentHomeworkDto dto, int userId);
        List<StudentHomeworkDto> GetStudentAnswerOnTaskAllowedToUser(int taskId, int userId);
        void CheckUserAccessToStudentAnswerByUserId(UserIdentityInfo userInfo, StudentHomeworkDto studentAnswerDto);
    }
}