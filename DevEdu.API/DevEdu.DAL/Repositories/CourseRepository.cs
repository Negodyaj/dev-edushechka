using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository : BaseRepository, ICourseRepository
    {
        public int AddCourse(CourseDto courseDto)
        {
            return _connection.QuerySingle<int>(
                "dbo.Course_Insert",
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
                "dbo.Course_Delete",
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CourseDto GetCourse(int id)
        {
            return _connection.QuerySingleOrDefault<CourseDto>(
                "dbo.Course_SelectById",
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CourseDto> GetCourses()
        {
            return _connection
                .Query<CourseDto>(
                    "dbo.Course_SelectAll",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateCourse(int id, CourseDto courseDto)
        {
            _connection.Execute(
                "dbo.Course_Update",
                new
                {
                    id,
                    courseDto.Name,
                    courseDto.Description
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}