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
using TaskStatus = DevEdu.DAL.Enums.StudentHomeworkStatus;

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

        public async Task<int> AddStudentHomeworkAsync(StudentHomeworkDto dto)
        {
            return await (_connection.QuerySingleAsync<int>(
                _studentHomeworkInsertProcedure,
                new
                {
                    HomeworkId = dto.Homework.Id,
                    StudentId = dto.User.Id,
                    dto.Answer
                },
                commandType: CommandType.StoredProcedure
            ));
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
                    dto.Answer
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ChangeStatusOfStudentAnswerOnTaskAsync(int id, int statusId, DateTime completedDate)
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
                .QueryAsync<StudentHomeworkDto, HomeworkDto, UserDto, TaskDto, TaskStatus, StudentHomeworkDto>(
                    _studentHomeworkSelectByIdProcedure,
                    (studentHomework, homework, user, task, studentHomeworkStatus) =>
                    {
                        studentHomework.Homework = homework;
                        studentHomework.User = user;
                        studentHomework.Homework.Task = task;
                        studentHomework.StudentHomeworkStatus = studentHomeworkStatus;

                        return studentHomework;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .FirstOrDefault();
            return result;
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByTaskAsync(int taskId)
        {
            return ( await _connection
                .QueryAsync<StudentHomeworkDto, TaskStatus, UserDto, StudentHomeworkDto>(
                _studentHomeworkSelectAllAnswersByTaskIdProcedure,
                (studentAnswer, studentHomeworkStatus, user) =>
                {
                    studentAnswer.StudentHomeworkStatus = studentHomeworkStatus;
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
            return ( await _connection
                .QueryAsync<StudentHomeworkDto, TaskStatus, HomeworkDto, TaskDto, StudentHomeworkDto>(
                    _studentHomeworkSelectAnswersByUserIdProcedure,
                    (answerDto, studentHomeworkStatus, homework, task) =>
                    {
                        answerDto.StudentHomeworkStatus = studentHomeworkStatus;
                        answerDto.Homework = homework;
                        answerDto.Homework.Task = task;
                        return answerDto;
                    },
                    new
                    {
                        userId
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }
    }
}