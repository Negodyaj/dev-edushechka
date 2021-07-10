using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository : BaseRepository, ICourseRepository
    {
        // todo: rename it
        private const string _insertProcedure = "dbo.Course_Insert";
        private const string _deleteProcedure = "dbo.Course_Delete";
        private const string _selectByIdProcedure = "dbo.Course_SelectById";
        private const string _selectAllProcedure = "dbo.Course_SelectAll";
        private const string _updateProcedure = "dbo.Course_Update";
        private const string _tagToTopicAddProcedure = "dbo.Tag_Topic_Insert";
        private const string _tagFromTopicDeleteProcedure = "dbo.Tag_Topic_Delete";
        private const string _insertCourseMaterial = "dbo.Course_Material_Insert";
        private const string _deleteCourseMaterial = "dbo.Course_Material_Delete";

        public CourseRepository()
        {
        }

        public int AddCourse(CourseDto courseDto)
        {
            return _connection.QuerySingle<int>(
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

        public void AddTagToTopic(int topicId, int tagId)
        {
            _connection.Query(
                _tagToTopicAddProcedure,
                new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public void DeleteTagFromTopic(int topicId, int tagId)
        {
            _connection.Query(
                _tagFromTopicDeleteProcedure,
                new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public int AddCourseMaterialReference(int courseId, int materialId)
        {
            return _connection.Execute(
                _insertCourseMaterial,
                new
                {
                    courseId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public int RemoveCourseMaterialReference(int courseId, int materialId)
        {
            return _connection.Execute(
                _deleteCourseMaterial,
                new
                {
                    courseId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}