using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository
    {
        private static string _connectionString =
            @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DevEdu.Db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        //private static string _connectionString =
        //    @"Data Source=80.78.240.16;Initial Catalog=DevEdu;Persist Security Info=True;User ID=student;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        private IDbConnection _connection = new SqlConnection(_connectionString);
        public int AddCourse(CourseDto courseDto)
        {
            return _connection.QuerySingle<int>("dbo.Course_Insert", new
                {
                    courseDto.Name,
                    courseDto.Description
            },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteCourse(int id)
        {
            _connection.Query("dbo.Course_Delete", new
                {
                    id
                },
                commandType: CommandType.StoredProcedure);
        }

        public CourseDto GetCourse(int id)
        {
            return _connection.QuerySingle<CourseDto>("dbo.Course_SelectById", new
                {
                    id
                },
                commandType: CommandType.StoredProcedure);
        }

        public List<CourseDto> GetCourses()
        {
            return _connection.Query<CourseDto>("dbo.Course_SelectAll",
                commandType: CommandType.StoredProcedure).AsList();
        }

        public void UpdateCourse(int id, CourseDto courseDto)
        {
            _connection.Query("dbo.Course_Update", new
                {
                    id,
                    courseDto.Name,
                    courseDto.Description
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}