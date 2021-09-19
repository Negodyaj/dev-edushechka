using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IStudentHomeworkService
    {
        Task<StudentHomeworkDto> AddStudentHomeworkAsync(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo);
        Task DeleteStudentHomeworkAsync(int id, UserIdentityInfo userInfo);
        Task<StudentHomeworkDto> UpdateStudentHomeworkAsync(int id, StudentHomeworkDto updatedDto, UserIdentityInfo userInfo);
        Task<int> UpdateStatusOfStudentHomeworkAsync(int id, int statusId, UserIdentityInfo userInfo);
        Task<StudentHomeworkDto> GetStudentHomeworkByIdAsync(int id, UserIdentityInfo userInfo);
        Task<List<StudentHomeworkDto>> GetAllStudentHomeworkOnTaskAsync(int taskId);
        Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByStudentIdAsync(int userId, UserIdentityInfo userInfo);
    }
}