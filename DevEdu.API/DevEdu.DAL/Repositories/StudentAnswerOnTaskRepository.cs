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
        public StudentAnswerOnTaskRepository()
        {

        }

        public void DeleteStudentAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse)
        {
            _dbconnection.Execute(
                "dbo.Task_Student_Delete",
                new
                {
                    studentResponse.TaskId,
                    studentResponse.StudentId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public string AddStudentAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse)
        {
            return _dbconnection.QuerySingle<string>(
                "dbo.Task_Student_Insert",
                new
                {
                    studentResponse.Answer
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswerOnTaskDto()
        {
            return _dbconnection.Query<StudentAnswerOnTaskDto>(
                "dbo.Task_Student_SelectAll",
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<StudentAnswerOnTaskDto> GetStudentAnswerByTaskIdAndStudentIdOnTaskDto(StudentAnswerOnTaskDto studentResponse)
        {
            return _dbconnection.Query<StudentAnswerOnTaskDto>(
                "dbo.Task_Student_SelectByTaskAndStudent",
                new
                {
                    studentResponse.TaskId,
                    studentResponse.StudentId
                },
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateStudentAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse)
        {
            _dbconnection.Query<StudentAnswerOnTaskDto>(
                "dbo.Task_Student_UpdateAnswer",
                new
                {
                    studentResponse.TaskId,
                    studentResponse.StudentId,
                    studentResponse.Answer
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public void UpdateStatusAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse)
        {
            _dbconnection.Query<StudentAnswerOnTaskDto>(
                "dbo.Task_Student_UpdateStatusId",
                new
                {
                    studentResponse.TaskId,
                    studentResponse.StudentId,
                    studentResponse.StatusId
                },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
