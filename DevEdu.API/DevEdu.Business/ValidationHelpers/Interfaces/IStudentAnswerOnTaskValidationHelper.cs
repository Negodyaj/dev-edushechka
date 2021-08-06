using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentAnswerOnTaskValidationHelper
    {
        void CheckStudentAnswerOnTaskExistence(int taskId, int userId);
        StudentAnswerOnTaskDto CheckStudentAnswerOnTaskExistence(int id);
        void CheckUserInStudentAnswerAccess(int studentId, int userId);
    }
}