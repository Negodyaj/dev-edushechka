using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentHomeworkService
    {
        int AddStudentAnswerOnTask(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo);
        void DeleteStudentAnswerOnTask(int id, UserIdentityInfo userInfo);
        StudentHomeworkDto UpdateStudentAnswerOnTask(int id, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo);
        int ChangeStatusOfStudentAnswerOnTask(int id, int statusId, UserIdentityInfo userInfo);
        StudentHomeworkDto GetStudentHomeworkId(int id, UserIdentityInfo userInfo);
        List<StudentHomeworkDto> GetAllStudentAnswersOnTask(int taskId, UserIdentityInfo userInfo);
        List<StudentHomeworkDto> GetAllAnswersByStudentId(int userId, UserIdentityInfo userInfo);
    }
}