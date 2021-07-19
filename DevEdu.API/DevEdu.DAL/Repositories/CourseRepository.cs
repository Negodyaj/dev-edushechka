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
        private const string _selectAllTopicsByCourseIdProcedure = "[dbo].[Course_Topic_SelectAllByCourseId]";
        private const string _updateCourseTopicsProcedure = "[dbo].[Course_Topic_Update]";
        private const string _deleteAllTopicsByCourseIdProcedure = "[dbo].[Course_Topic_DeleteAllTopicsByCourseId]";
        private const string _course_TopicType = "dbo.Course_TopicType";

        private const string _insertCourseMaterial = "dbo.Course_Material_Insert";
        private const string _deleteCourseMaterial = "dbo.Course_Material_Delete";

        private const string _сourseTaskInsertProcedure = "dbo.Course_Task_Insert";
        private const string _сourseTaskDeleteProcedure = "dbo.Course_Task_Delete";

        private const string _courseSelectByTaskIdProcedure = "dbo.Course_SelectByTaskId";

        public CourseRepository()
        {
        }

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
            CourseDto result = default;
            _connection.Query<CourseDto, GroupDto, CourseDto>(
                _courseSelectByIdProcedure,
                (course, group) =>
                {
                    if (result == null)
                    {
                        result = course;
                        result.Groups = new List<GroupDto> { group };
                    }
                    else
                    {
                        result.Groups.Add(group);
                    }
                    return result;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
            return result;
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

        public void AddTaskToCourse(int courseId, int taskId)
        {
            _connection.Execute(
                _сourseTaskInsertProcedure,
                new
                {
                    taskId,
                    courseId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteTaskFromCourse(int courseId, int taskId)
        {
            _connection.Execute(
                _сourseTaskDeleteProcedure,
                new
                {
                    taskId,
                    courseId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId)
        {
            return _connection
                .Query<CourseTopicDto, TopicDto, CourseTopicDto>(
                    _selectAllTopicsByCourseIdProcedure,
                    (courseTopicDto, topicDto) =>
                    {
                        courseTopicDto.Topic = topicDto;
                        courseTopicDto.Course = new CourseDto() { Id = courseId };
                        return courseTopicDto;
                    },
                    new { courseId },
                    splitOn: "id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }
        public void UpdateCourseTopicsByCourseId(List<CourseTopicDto> topics)
        {
            var dt = new DataTable();
            dt.Columns.Add("CourseId");
            dt.Columns.Add("TopicId");
            dt.Columns.Add("Position");

            foreach (var topic in topics)
            {
                dt.Rows.Add(topic.Course.Id, topic.Topic.Id, topic.Position);
            }
            _connection.Execute(
                _updateCourseTopicsProcedure,
                new { tblCourseTopic = dt.AsTableValuedParameter(_course_TopicType) },
                commandType: CommandType.StoredProcedure
                );
        }
        public void DeleteAllTopicsByCourseId(int courseId)
        {
            _connection.Execute(
                _deleteAllTopicsByCourseIdProcedure,
                new { courseId },
                commandType: CommandType.StoredProcedure
                );
        }

        public List<CourseDto> GetCoursesToTaskByTaskId(int id)
        {
            return _connection.Query<CourseDto>(
                    _courseSelectByTaskIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
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