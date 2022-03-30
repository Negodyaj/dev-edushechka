using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentHomeworkRepository
    {
        Task<int> AddStudentHomeworkAsync(StudentHomeworkDto dto);
        Task<int> ChangeStatusOfStudentAnswerOnTaskAsync(int id, int statusId, DateTime completedDate, int rating);
        Task DeleteStudentHomeworkAsync(int id);
        Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByStudentIdAsync(int userId);
        Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByTaskAsync(int taskId);
        Task<StudentHomeworkDto> GetStudentHomeworkByIdAsync(int id);
        Task UpdateStudentHomeworkAsync(StudentHomeworkDto dto);
    }
}