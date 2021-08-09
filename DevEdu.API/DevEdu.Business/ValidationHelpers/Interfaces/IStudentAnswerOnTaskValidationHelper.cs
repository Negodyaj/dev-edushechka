using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentAnswerOnTaskValidationHelper
    {
        void CheckStudentAnswerOnTaskExistence(StudentAnswerOnTaskDto dto);
        StudentAnswerOnTaskDto CheckStudentAnswerOnTaskExistence(int id);
        void CheckUserInStudentAnswerAccess(int studentId, int userId);
    }
}