using System.Collections.Generic;
using System.Data;
using Dapper;
using DevEdu.DAL.Models;
using System.Linq;
using DevEdu.DAL.Enums;
using System;

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
        private const string _taskStudentSelectAllAnswersByTaskId = "dbo.Task_Student_SelectAllAnswersByTaskId";

        private const string _taskStudentCommentInsert = "dbo.Task_Student_Comment_Insert";
        private const string _task_Student_SelectByTaskIdProcedure = "dbo.Task_Student_SelectByTaskId";

        public StudentAnswerOnTaskRepository()
        {

        }

        public void DeleteStudentAnswerOnTask(StudentAnswerOnTaskDto dto)
        {
            _connection.Execute(
                _taskStudentDelete,
                new
                {
                    TaskId = dto.Task.Id,
                    StudentId = dto.User.Id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddStudentAnswerOnTask(StudentAnswerOnTaskDto dto)
        {
            _connection.QuerySingle<string>(
                _taskStudentInsert,
                new
                {
                    TaskId = dto.Task.Id,
                    StudentId = dto.User.Id,
                    Answer = dto.Answer
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks()
        {
            return _connection
                .Query<StudentAnswerOnTaskDto, TaskStatus, UserDto, StudentAnswerOnTaskDto>(
                _taskStudentSelectAll,
                (studentAnswer, taskStatus, user) =>
                {
                    studentAnswer.TaskStatus = taskStatus;
                    studentAnswer.User = user;

                    return studentAnswer;
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId)
        {
            return _connection
                .Query<StudentAnswerOnTaskDto, TaskStatus, UserDto, StudentAnswerOnTaskDto>(
                _taskStudentSelectAllAnswersByTaskId,
                (studentAnswer, taskStatus, user) =>
                {
                    studentAnswer.TaskStatus = taskStatus;
                    studentAnswer.User = user;

                    return studentAnswer;
                },
                new
                {
                    taskId
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(StudentAnswerOnTaskDto dto)
        {
            var result = _connection
                .Query<StudentAnswerOnTaskDto, UserDto, TaskDto, TaskStatus, StudentAnswerOnTaskDto>(
                _taskStudentSelectByTaskAndStudent,
                (studentAnswer, user, task, taskStatus) =>
                {
                    studentAnswer.User = user;
                    studentAnswer.Task = task;
                    studentAnswer.TaskStatus = taskStatus;

                    return studentAnswer;
                },
                new
                {
                    TaskId = dto.Task.Id,
                    StudentId = dto.User.Id
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
             )
             .FirstOrDefault();

            return result;
        }

        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto)
        {
            _connection.Execute(
                _taskStudentUpdateAnswer,
                new
                {
                    TaskId = dto.Task.Id,
                    StudentId = dto.User.Id,
                    dto.Answer
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public void ChangeStatusOfStudentAnswerOnTask(StudentAnswerOnTaskDto dto)
        {
            _connection.Execute(
                _taskStudentUpdateStatusId,
                new
                {
                    TaskId = dto.Task.Id,
                    StudentId = dto.User.Id,
                    StatusId = (int)(dto.TaskStatus),
                    dto.CompletedDate
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

        public List<StudentAnswerOnTaskForTaskDto> GetStudentAnswersToTaskByTaskId(int id)
        {
            return _connection.Query<StudentAnswerOnTaskForTaskDto, UserDto, StudentAnswerOnTaskForTaskDto>(
                    _task_Student_SelectByTaskIdProcedure,
                    (answerDto, userDto) =>
                    {
                        answerDto.Student = userDto;
                        return answerDto;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure)
                .ToList();
        }
    }
}