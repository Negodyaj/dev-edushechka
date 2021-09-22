using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository : BaseRepository, ICourseRepository

    {
        private const string _courseAddProcedure = "dbo.Course_Insert";
        private const string _courseDeleteProcedure = "dbo.Course_Delete";
        private const string _courseSelectByIdProcedure = "dbo.Course_SelectById";
        private const string _courseSelectAllProcedure = "dbo.Course_SelectAll";
        private const string _courseUpdateProcedure = "dbo.Course_Update";
        private const string _courseTopicSelectAllByCourseIdProcedure = "dbo.Course_Topic_SelectAllByCourseId";
        private const string _courseTopicUpdateProcedure = "dbo.Course_Topic_Update";
        private const string _courseTopicDeleteAllTopicsByCourseIdProcedure = "dbo.Course_Topic_DeleteAllTopicsByCourseId";
        private const string _courseTopicType = "dbo.Course_TopicType";

        private const string _courseMaterialInsertProcedure = "dbo.Course_Material_Insert";
        private const string _courseMaterialDeleteProcedure = "dbo.Course_Material_Delete";

        private const string _сourseTaskInsertProcedure = "dbo.Course_Task_Insert";
        private const string _сourseTaskDeleteProcedure = "dbo.Course_Task_Delete";

        private const string _courseSelectByTaskIdProcedure = "dbo.Course_SelectByTaskId";
        private const string _courseSelectAllByMaterialIdProcedure = "dbo.Course_SelectByMaterialId";

        public CourseRepository(IOptions<DatabaseSettings> options) : base(options) 
        {
        }

        public async Task<int> AddCourseAsync(CourseDto courseDto)
        {
            return await _connection.QuerySingleAsync<int>(
                _courseAddProcedure,
                new
                {
                    courseDto.Name,
                    courseDto.Description
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _connection.ExecuteAsync(
                _courseDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<CourseDto> GetCourseAsync(int id)
        {
            CourseDto result = default;

            return (await _connection
                .QueryAsync<CourseDto, TopicDto, CourseDto>(
                _courseSelectByIdProcedure,
                (course, topic) =>
                {
                    if (result == null)
                    {
                        result = course;
                        result.Topics = new List<TopicDto> { topic };
                    }
                    else
                    {
                        result.Topics.Add(topic);
                    }
                    return result;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                ))
                .FirstOrDefault();
        }

        public async Task<List<CourseDto>> GetCoursesAsync()
        {
            var courseDictionary = new Dictionary<int, CourseDto>();
            return (await _connection
                .QueryAsync<CourseDto, TopicDto, CourseDto>(
                    _courseSelectAllProcedure,
                    (course, topic) =>
                    {
                        if (!courseDictionary.TryGetValue(course.Id, out CourseDto result))
                        {
                            result = course;
                            result.Topics = new List<TopicDto> { topic };
                            courseDictionary.Add(course.Id, result);
                        }
                        else
                        {
                            result.Topics.Add(topic);
                        }

                        return result;
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .Distinct()
                .ToList();
        }

        public async Task UpdateCourseAsync(CourseDto courseDto)
        {
            await _connection.ExecuteAsync(
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

        public async Task AddTaskToCourseAsync(int courseId, int taskId)
        {
            await _connection.ExecuteAsync(
                _сourseTaskInsertProcedure,
                new
                {
                    taskId,
                    courseId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteTaskFromCourseAsync(int courseId, int taskId)
        {
            await _connection.ExecuteAsync(
                _сourseTaskDeleteProcedure,
                new
                {
                    taskId,
                    courseId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<CourseTopicDto>> SelectAllTopicsByCourseIdAsync(int courseId)
        {
            return (await _connection
                .QueryAsync<CourseTopicDto, TopicDto, CourseTopicDto>(
                    _courseTopicSelectAllByCourseIdProcedure,
                    (courseTopicDto, topicDto) =>
                    {
                        courseTopicDto.Topic = topicDto;
                        courseTopicDto.Course = new CourseDto() { Id = courseId };
                        return courseTopicDto;
                    },
                    new { courseId },
                    splitOn: "id",
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task UpdateCourseTopicsByCourseId(List<CourseTopicDto> topics)
        {
            var dt = new DataTable();
            dt.Columns.Add("CourseId");
            dt.Columns.Add("TopicId");
            dt.Columns.Add("Position");

            foreach (var topic in topics)
            {
                dt.Rows.Add(topic.Course.Id, topic.Topic.Id, topic.Position);
            }

            await _connection.ExecuteAsync(
                _courseTopicUpdateProcedure,
                new { tblCourseTopic = dt.AsTableValuedParameter(_courseTopicType) },
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task DeleteAllTopicsByCourseIdAsync(int courseId)
        {
            await _connection.ExecuteAsync(
                _courseTopicDeleteAllTopicsByCourseIdProcedure,
                new { courseId },
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<List<CourseDto>> GetCoursesToTaskByTaskIdAsync(int id)
        {
            return (await _connection.QueryAsync<CourseDto>(
                    _courseSelectByTaskIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task<List<CourseDto>> GetCoursesByMaterialIdAsync(int id)
        {
            return (await _connection.QueryAsync<CourseDto>(
                    _courseSelectAllByMaterialIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task<int> AddCourseMaterialReferenceAsync(int courseId, int materialId)
        {
            return await _connection.ExecuteAsync(
                _courseMaterialInsertProcedure,
                new
                {
                    courseId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RemoveCourseMaterialReferenceAsync(int courseId, int materialId)
        {
            await _connection.ExecuteAsync(
               _courseMaterialDeleteProcedure,
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