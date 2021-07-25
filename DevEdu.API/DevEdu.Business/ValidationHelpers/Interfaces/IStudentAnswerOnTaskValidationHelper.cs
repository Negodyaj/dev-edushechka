using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentAnswerOnTaskValidationHelper
    {
        void CheckStudentAnswerOnTaskExistence(StudentAnswerOnTaskDto dto);
    }
}