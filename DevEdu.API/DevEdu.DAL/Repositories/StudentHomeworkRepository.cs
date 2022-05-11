using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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
        private const string _studentHomeworkSelectAnswerByTaskIdAndUserIdProcedure = "Student_Homework_SelectAnswerByTaskIdAndUserId";

        public StudentHomeworkRepository(IOptions<DatabaseSettings> options) : base(options) 
        {
        }

        public async Task<int> AddStudentHomeworkAsync(StudentHomeworkDto dto)
        {
            return await _connection.QuerySingleAsync<int>(
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

        public async Task DeleteStudentHomeworkAsync(int id)
        {
            await _connection.ExecuteAsync(
                 _studentHomeworkDeleteProcedure,
                 new { id },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task UpdateStudentHomeworkAsync(StudentHomeworkDto dto)
        {
            await _connection.ExecuteAsync(
                 _studentHomeworkUpdateAnswerProcedure,
                 new
                 {
                     dto.Id,
                     dto.Answer,
                     dto.Status
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task<int> ChangeStatusOfStudentAnswerOnTaskAsync(int id, int statusId, DateTime? completedDate)
        {
            await _connection.ExecuteAsync(
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

        public async Task<StudentHomeworkDto> GetStudentHomeworkByIdAsync(int id)
        {
            var result = (await _connection
                .QueryAsync<StudentHomeworkDto, HomeworkDto, UserDto, TaskDto, StudentHomeworkStatus, StudentHomeworkDto>(
                    _studentHomeworkSelectByIdProcedure,
                    (studentHomework, homework, user, task, studentHomeworkStatus) =>
                    {
                        studentHomework.Homework = homework;
                        studentHomework.User = user;
                        studentHomework.Homework.Task = task;
                        studentHomework.Status = studentHomeworkStatus;

                        return studentHomework;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .FirstOrDefault();
            return result;
        }

        public async Task<StudentHomeworkDto> GetStudentHomeworkByTaskIdAndUserId(int taskId, int userId)
        {
            var result = (await _connection
                .QueryAsync<StudentHomeworkDto, HomeworkDto, TaskDto, StudentHomeworkStatus, StudentHomeworkDto>(
                    _studentHomeworkSelectAnswerByTaskIdAndUserIdProcedure,
                    (studentHomework, homework, task, studentHomeworkStatus) =>
                    {
                        studentHomework.Homework = homework;
                        studentHomework.Homework.Task = task;
                        studentHomework.Status = studentHomeworkStatus;

                        return studentHomework;
                    },
                    new { taskId, userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .FirstOrDefault();
            return result;
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByTaskAsync(int taskId)
        {
            return (await _connection
                .QueryAsync<StudentHomeworkDto, StudentHomeworkStatus, UserDto, StudentHomeworkDto>(
                _studentHomeworkSelectAllAnswersByTaskIdProcedure,
                (studentAnswer, studentHomeworkStatus, user) =>
                {
                    studentAnswer.Status = studentHomeworkStatus;
                    studentAnswer.User = user;

                    return studentAnswer;
                },
                new
                {
                    taskId
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByStudentIdAsync(int userId)
        {
            return (await _connection
                .QueryAsync<StudentHomeworkDto, StudentHomeworkStatus, HomeworkDto, TaskDto, StudentHomeworkDto>(
                    _studentHomeworkSelectAnswersByUserIdProcedure,
                    (answerDto, studentHomeworkStatus, homework, task) =>
                    {
                        answerDto.Status = studentHomeworkStatus;
                        answerDto.Homework = homework;
                        answerDto.Homework.Task = task;
                        return answerDto;
                    },
                    new
                    {
                        userId
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure))
                .ToList();
        }
    }
}