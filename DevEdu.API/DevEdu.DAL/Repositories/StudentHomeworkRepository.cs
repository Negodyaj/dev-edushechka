using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class StudentHomeworkRepository : BaseRepository, IStudentHomeworkRepository
    {
        private const string _studentHomeworkInsertProcedure = "dbo.Student_Homework_Insert";
        private const string _studentHomeworkDeleteProcedure = "dbo.Student_Homework_Delete";
        private const string _studentHomeworkUpdateAnswerProcedure = "dbo.Student_Homework_UpdateAnswer";
        private const string _studentHomeworkUpdateStatusIdProcedure = "dbo.Student_Homework_UpdateStatusId";
        private const string _studentHomeworkSelectByIdProcedure = "dbo.Student_Homework_SelectById";
        private const string _studentHomeworkSelectAllAnswersByTaskIdProcedure = "dbo.Student_Homework_SelectAllAnswersByTaskId";
        private const string _studentHomeworkSelectAnswersByUserIdProcedure = "dbo.Student_Homework_SelectAllAnswersByUserId";

        public StudentHomeworkRepository(IOptions<DatabaseSettings> options) : base(options) { }

        public int AddStudentHomework(StudentHomeworkDto dto)
        {
            return _connection.QuerySingle<int>(
                _studentHomeworkInsertProcedure,
                new
                {
                    HomeworkId = dto.Homework.Id,
                    StudentId = dto.User.Id,
                    dto.Answer
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteStudentHomework(int id)
        {
            _connection.Execute(
                _studentHomeworkDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateStudentHomework(StudentHomeworkDto dto)
        {
            _connection.Execute(
                _studentHomeworkUpdateAnswerProcedure,
                new
                {
                    dto.Id,
                    dto.Answer
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int ChangeStatusOfStudentAnswerOnTask(int id, int statusId, DateTime completedDate)
        {
            _connection.Execute(
                _studentHomeworkUpdateStatusIdProcedure,
                new
                {
                    id,
                    StatusId = statusId,
                    CompletedDate = completedDate
                },
                commandType: CommandType.StoredProcedure
            );

            return statusId;
        }

        public StudentHomeworkDto GetStudentHomeworkById(int id)
        {
            var result = _connection
                .Query<StudentHomeworkDto, UserDto, HomeworkDto, TaskDto, TaskStatus, StudentHomeworkDto>(
                    _studentHomeworkSelectByIdProcedure,
                    (studentAnswer, user, homework, task, taskStatus) =>
                    {
                        studentAnswer.User = user;
                        studentAnswer.Homework = homework;
                        studentAnswer.Homework.Task = task;
                        studentAnswer.TaskStatus = taskStatus;

                        return studentAnswer;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
            return result;
        }

        public List<StudentHomeworkDto> GetAllStudentHomeworkByTask(int taskId)
        {
            return _connection
                .Query<StudentHomeworkDto, TaskStatus, UserDto, StudentHomeworkDto>(
                _studentHomeworkSelectAllAnswersByTaskIdProcedure,
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

        public List<StudentHomeworkDto> GetAllStudentHomeworkByStudentId(int userId)
        {
            return _connection
                .Query<StudentHomeworkDto, TaskStatus, HomeworkDto, TaskDto, StudentHomeworkDto>(
                    _studentHomeworkSelectAnswersByUserIdProcedure,
                    (answerDto, taskStatus, homework, task) =>
                    {
                        answerDto.TaskStatus = taskStatus;
                        answerDto.Homework = homework;
                        answerDto.Homework.Task = task;
                        return answerDto;
                    },
                    new
                    {
                        userId
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure)
                .ToList();
        }
    }
}