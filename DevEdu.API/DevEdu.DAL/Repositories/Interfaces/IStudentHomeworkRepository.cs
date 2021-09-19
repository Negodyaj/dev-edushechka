using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentHomeworkRepository
    {
        Task<int> AddStudentHomeworkAsync(StudentHomeworkDto dto);
        Task DeleteStudentHomeworkAsync(int id);
        Task UpdateStudentHomeworkAsync(StudentHomeworkDto dto);
        Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByTaskAsync(int taskId);
        Task<int> ChangeStatusOfStudentAnswerOnTaskAsync(int id, int statusId, DateTime completedDate);
        Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByStudentIdAsync(int userId);
        Task<StudentHomeworkDto> GetStudentHomeworkByIdAsync(int id);
    }
}