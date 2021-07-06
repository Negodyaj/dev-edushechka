using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class StudentAnswerOnTaskRepository : BaseRepository, IStudentAnswerOnTaskRepository
    {
        private const string _taskStudentDelete = "dbo.Task_Student_Delete";
        private const string _taskStudentInsert = "dbo.Task_Student_Insert";
        private const string _taskStudentSelectAll = "dbo.Task_Student_SelectAll";
        private const string _taskStudentSelectByTaskAndStudent = "dbo.Task_Student_SelectByTaskAndStudent";
        private const string _taskStudentUpdateAnswer = "dbo.Task_Student_UpdateAnswer";
        private const string _taskStudentUpdateStatusId = "dbo.Task_Student_UpdateStatusId";


        public StudentAnswerOnTaskRepository()
        {

        }

        public void DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            _dbconnection.Execute(
                _taskStudentDelete,
                new
                {
                    taskId,
                    studentId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse)
        {
            _dbconnection.QuerySingle<string>(
                _taskStudentInsert,
                new
                {
                    studentResponse.Answer
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswerOnTask()
        {
            return _dbconnection.Query<StudentAnswerOnTaskDto>(
                _taskStudentSelectAll,
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<StudentAnswerOnTaskDto> GetStudentAnswerByTaskIdAndStudentIdOnTask(StudentAnswerOnTaskDto studentResponse)
        {
            return _dbconnection.Query<StudentAnswerOnTaskDto>(
                _taskStudentSelectByTaskAndStudent,
                new
                {
                    studentResponse.TaskId,
                    studentResponse.StudentId
                },
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse)
        {
            _dbconnection.Query<StudentAnswerOnTaskDto>(
                _taskStudentUpdateAnswer,
                new
                {
                    studentResponse.TaskId,
                    studentResponse.StudentId,
                    studentResponse.Answer
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public void UpdateStatusAnswerOnTask(int taskId, int studentId, int statusId)
        {
            _dbconnection.Query<StudentAnswerOnTaskDto>(
                _taskStudentUpdateStatusId,
                new
                {
                    taskId,
                    studentId,
                    statusId
                },
                commandType: CommandType.StoredProcedure
                );
        }

    }
}
