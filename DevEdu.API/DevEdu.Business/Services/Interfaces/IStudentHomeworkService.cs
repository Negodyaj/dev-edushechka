using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IStudentHomeworkService
    {
        StudentHomeworkDto AddStudentHomework(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo);
        void DeleteStudentHomework(int id, UserIdentityInfo userInfo);
        StudentHomeworkDto UpdateStudentHomework(int id, StudentHomeworkDto updatedDto, UserIdentityInfo userInfo);
        int UpdateStatusOfStudentHomework(int id, int statusId, UserIdentityInfo userInfo);
        StudentHomeworkDto GetStudentHomeworkById(int id, UserIdentityInfo userInfo);
        Task<List<StudentHomeworkDto>> GetAllStudentHomeworkOnTaskAsync(int taskId);
        List<StudentHomeworkDto> GetAllStudentHomeworkByStudentId(int userId, UserIdentityInfo userInfo);
    }
}