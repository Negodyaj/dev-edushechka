using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentAnswerOnTaskValidationHelper
    {
        void CheckStudentAnswerOnTaskExistence(int taskId, int userId);
        StudentAnswerOnTaskDto CheckStudentAnswerOnTaskExistence(int id);
        void CheckUserInStudentAnswerAccess(int studentId, int userId);
        void CheckUserComplianceToStudentAnswer(StudentAnswerOnTaskDto dto, int userId);
        StudentAnswerOnTaskDto GetStudentAnswerByTaskIdAndStudentIdOrThrowIfNotFound(int taskId, int studentId);
        List<StudentAnswerOnTaskDto> GetStudentAnswerOnTaskAllowedToUser(int taskId, int userId);
        void CheckUserAccessToStudentAnswerByUserId(UserIdentityInfo userInfo, StudentAnswerOnTaskDto studentAnswerDto);
    }
}