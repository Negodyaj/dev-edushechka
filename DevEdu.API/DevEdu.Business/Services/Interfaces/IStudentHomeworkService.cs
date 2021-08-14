using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentHomeworkService
    {
        int AddStudentAnswerOnTask(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo);
        void DeleteStudentAnswerOnTask(int id, UserIdentityInfo userInfo);
        StudentHomeworkDto UpdateStudentAnswerOnTask(int id, StudentHomeworkDto updatedDto, UserIdentityInfo userInfo);
        int ChangeStatusOfStudentAnswerOnTask(int id, int statusId, UserIdentityInfo userInfo);
        StudentHomeworkDto GetStudentHomeworkById(int id, UserIdentityInfo userInfo);
        List<StudentHomeworkDto> GetAllStudentAnswersOnTask(int taskId);
        List<StudentHomeworkDto> GetAllAnswersByStudentId(int userId, UserIdentityInfo userInfo);
    }
}