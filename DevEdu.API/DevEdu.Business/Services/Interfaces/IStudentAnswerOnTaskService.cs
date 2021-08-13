using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentAnswerOnTaskService
    {
        int AddStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto, UserIdentityInfo userInfo);
        void DeleteStudentAnswerOnTask(int taskId, int studentId, UserIdentityInfo userInfo);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId, UserIdentityInfo userInfo);
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId, UserIdentityInfo userInfo);
        int ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId, UserIdentityInfo userInfo);
        StudentAnswerOnTaskDto UpdateStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto, UserIdentityInfo userInfo);
        List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId, UserIdentityInfo userInfo);
    }
}