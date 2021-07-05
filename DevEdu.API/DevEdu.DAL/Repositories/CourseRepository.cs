using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository : BaseRepository, ICourseRepository
    {
        public CourseRepository()
        {
            _insertProcedure = "dbo.Course_Insert";
            _deleteProcedure = "dbo.Course_Delete";
            _selectByIdProcedure = "dbo.Course_SelectById";
            _selectAllProcedure = "dbo.Course_SelectAll";
            _updateProcedure = "dbo.Course_Update";
        }

        public int AddCourse(CourseDto courseDto)
        {
            return _connection.QuerySingleOrDefault<int>(
                _insertProcedure,
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
                _deleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CourseDto GetCourse(int id)
        {
            return _connection.QuerySingleOrDefault<CourseDto>(
                _selectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CourseDto> GetCourses()
        {
            return _connection
                .Query<CourseDto>(
                    _selectAllProcedure,
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateCourse(CourseDto courseDto)
        {
            _connection.Execute(
                _updateProcedure,
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
