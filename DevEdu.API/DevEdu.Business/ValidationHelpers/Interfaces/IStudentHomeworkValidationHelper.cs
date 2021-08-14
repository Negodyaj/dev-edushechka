using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentHomeworkValidationHelper
    {
        StudentHomeworkDto GetStudentAnswerByIdAndThrowIfNotFound(int id);
        void CheckUserInStudentHomeworkAccess(int studentId, int userId);
        void CheckUserBelongsToHomework(int groupId, int userId);
        void CheckUserComplianceToStudentHomework(int studentId, int userId);
    }
}