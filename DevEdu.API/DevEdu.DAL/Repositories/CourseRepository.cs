using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository : BaseRepository, ICourseRepository
    {
        private const string _courseAddProcedure = "dbo.Course_Insert";
        private const string _courseDeleteProcedure = "dbo.Course_Delete";
        private const string _courseSelectByIdProcedure = "dbo.Course_SelectById";
        private const string _courseSelectAllProcedure = "dbo.Course_SelectAll";
        private const string _courseUpdateProcedure = "dbo.Course_Update";

        public CourseRepository() { }

        public int AddCourse(CourseDto courseDto)
        {
            return _connection.QuerySingle<int>(
                _courseAddProcedure,
                new
                {
                    courseDto.Name,
                    courseDto.Description
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteCourse(int id)
        {
            _connection.Execute(
                _courseDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CourseDto GetCourse(int id)
        {
            return _connection.QuerySingleOrDefault<CourseDto>(
                _courseSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CourseDto> GetCourses()
        {
            return _connection
                .Query<CourseDto>(
                    _courseSelectAllProcedure,
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateCourse(CourseDto courseDto)
        {
            _connection.Execute(
                _courseUpdateProcedure,
                new
                {
                    courseDto.Id,
                    courseDto.Name,
                    courseDto.Description
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}