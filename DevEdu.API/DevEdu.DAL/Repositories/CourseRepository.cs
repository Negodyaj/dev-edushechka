using System;
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
        private const string _tagToTopicAddProcedure = "dbo.Tag_Topic_Insert";
        private const string _tagFromTopicDeleteProcedure = "dbo.Tag_Topic_Delete";
        private const string _selectAllTopicsByCourseIdProcedure = "[dbo].[Course_Topic_SelectAllByCourseId]";

        private const string _сourseTaskInsertProcedure = "dbo.Course_Task_Insert";
        private const string _сourseTaskDeleteProcedure = "dbo.Course_Task_Delete";

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
            return _connection
                .Query<CourseDto, GroupDto, CourseDto>(
                _courseSelectByIdProcedure,
                (course, group) =>
                {
                    if (result == null)
                    {
                        result = course;
                        result.Groups = new List<GroupDto> {group};
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
        }

        public List<CourseDto> GetCourses()
        {
            var courseDictionary = new Dictionary<int, CourseDto>();
            CourseDto result;
            return _connection
                .Query<CourseDto, GroupDto, CourseDto>(
                    _courseSelectAllProcedure,
                    (course, group) =>
                    {

                        if (!courseDictionary.TryGetValue(course.Id, out result))
                        {
                            result = course;
                            result.Groups = new List<GroupDto> { group };
                            courseDictionary.Add(course.Id, result);
                        }
                        else
                        {
                            result.Groups.Add(group);
                        }

                        return result;
                    },
                    splitOn: "CourseId",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
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
                .Query<CourseTopicDto,TopicDto, CourseTopicDto>(
                    _selectAllTopicsByCourseIdProcedure,
                    (courseTopicDto, topicDto) =>
                    {
                        courseTopicDto.Topic = topicDto;
                        courseTopicDto.Course = new CourseDto() { Id = courseId };
                        return courseTopicDto;
                    },
                    new {courseId},
                    splitOn: "id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }
    }
}