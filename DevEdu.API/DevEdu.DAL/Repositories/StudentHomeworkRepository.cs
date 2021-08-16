using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class StudentHomeworkRepository : BaseRepository, IStudentHomeworkRepository
    {
        private const string _studentHomeworkInsert = "dbo.Student_Homework_Insert";
        private const string _studentHomeworkDelete = "dbo.Student_Homework_Delete";
        private const string _studentHomeworkUpdateAnswer = "dbo.Student_Homework_UpdateAnswer";
        private const string _studentHomeworkUpdateStatusId = "dbo.Student_Homework_UpdateStatusId";
        private const string _studentHomeworkSelectById = "dbo.Student_Homework_SelectById";
        private const string _studentHomeworkSelectAllAnswersByTaskId = "dbo.Student_Homework_SelectAllAnswersByTaskId";
        private const string _studentHomeworkSelectAnswersByUserId = "dbo.Student_Homework_SelectAllAnswersByUserId";

        public StudentHomeworkRepository()
        {

        }

        public int AddStudentHomework(StudentHomeworkDto dto)
        {
            return _connection.QuerySingle<int>(
                _studentHomeworkInsert,
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
                _studentHomeworkDelete,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateStudentHomework(StudentHomeworkDto dto)
        {
            _connection.Execute(
                _studentHomeworkUpdateAnswer,
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
                _studentHomeworkUpdateStatusId,
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
                    _studentHomeworkSelectById,
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
                _studentHomeworkSelectAllAnswersByTaskId,
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
                    _studentHomeworkSelectAnswersByUserId,
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