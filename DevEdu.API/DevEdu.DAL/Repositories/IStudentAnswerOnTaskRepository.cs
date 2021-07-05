using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentAnswerOnTaskRepository
    {
        string AddStudentAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse);
        void DeleteStudentAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswerOnTaskDto();
        List<StudentAnswerOnTaskDto> GetStudentAnswerByTaskIdAndStudentIdOnTaskDto(StudentAnswerOnTaskDto studentResponse);
        void UpdateStatusAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse);
        void UpdateStudentAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse);
    }
}