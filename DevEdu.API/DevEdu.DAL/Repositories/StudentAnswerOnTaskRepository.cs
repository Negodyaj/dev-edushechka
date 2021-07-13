using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;
using System.Linq;
using DevEdu.DAL.Enums;

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

        private const string _taskStudentCommentInsert = "dbo.Task_Student_Comment_Insert";

        public StudentAnswerOnTaskRepository()
        {

        }

        public void DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            _connection.Execute(
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
            _connection.QuerySingle<string>(
                _taskStudentInsert,
                new
                {
                    studentResponse.Answer
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask()
        {
            var studentStudentDictionary = new Dictionary<int, StudentAnswerOnTaskDto>();

            return _connection
                .Query<StudentAnswerOnTaskDto, TaskStatus, StudentAnswerOnTaskDto>(
                _taskStudentSelectAll,
                (studentAnswer, taskStatus) =>
                {
                    StudentAnswerOnTaskDto studentAnswerEntry;

                    if (!studentStudentDictionary.TryGetValue(studentAnswer.Id, out studentAnswerEntry))
                    {
                        studentAnswerEntry = studentAnswer;
                        studentAnswer.TaskStatus = taskStatus;
                        studentStudentDictionary.Add(studentAnswer.Id, studentAnswerEntry);
                    }

                    return studentAnswerEntry;
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        public StudentAnswerOnTaskDto GetStudentAnswerByTaskIdAndStudentIdOnTask(int taskId, int studentId)
        {
            StudentAnswerOnTaskDto result = default;

            _connection.
                Query<StudentAnswerOnTaskDto, CommentDto, TaskStatus, StudentAnswerOnTaskDto>(
                _taskStudentSelectByTaskAndStudent,
                (studentAnswer, comment, taskStatus) =>
                {
                    if(result == null)
                    {
                        result = studentAnswer;
                        result.Comments = new List<CommentDto> { comment };
                        result.TaskStatus = taskStatus;
                    }
                    else
                    {
                        result.Comments.Add(comment);
                    }

                    return studentAnswer;
                },
                new
                {
                    taskId,
                    studentId
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
             )
             .FirstOrDefault();

            return result;
        }

        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse)
        {
            _connection.Query<StudentAnswerOnTaskDto>(
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
            _connection.Query<StudentAnswerOnTaskDto>(
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

        public void AddCommentOnStudentAnswer(int taskstudentId, int commentId)
        {
            _connection.QuerySingle<int>(
                _taskStudentCommentInsert,
                new
                {
                    taskstudentId,
                    commentId
                },
                commandType: CommandType.StoredProcedure
           );
        }
    }
}
