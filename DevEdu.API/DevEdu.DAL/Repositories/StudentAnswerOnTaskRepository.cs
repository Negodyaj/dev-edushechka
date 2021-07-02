using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class StudentAnswerOnTaskRepository
    {
        public string connectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";
        private IDbConnection _dbconnection;

        public StudentAnswerOnTaskRepository()
        {
            _dbconnection = new SqlConnection(connectionString);
        }


        public void DeleteStudentAnswerOnTaskDto(StudentAnswerOnTaskDto studentResponse)
        {
            string query = "exec dbo.Task_Student_Delete @TaskId, @StudentId";
            _dbconnection.Query(query, new { studentResponse.TaskId, studentResponse.StudentId }, commandType: CommandType.StoredProcedure);
        }

        public string AddStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse)
        {
            string query = "exec dbo.Task_Student_Insert";
            return _dbconnection.QuerySingle<string>(query, new { studentResponse.Answer },
                                                     commandType: CommandType.StoredProcedure);
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswerOnTaskDto()
        {
            string query = "exec dbo.Task_Student_SelectAll";
            return _dbconnection.Query<StudentAnswerOnTaskDto>(query, commandType: CommandType.StoredProcedure).AsList(); ;
        }


    }
}
