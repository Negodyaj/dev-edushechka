using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class TopicRepository : BaseRepository, ITopicRepository
    {
        private const string _topicInsertProcedure = "dbo.Topic_Insert";
        private const string _topicDeleteProcedure = "dbo.Topic_Delete";
        private const string _topicSelectByIdProcedure = "dbo.Topic_SelectById";
        private const string _topicSelectByCourseIdProcedure = "dbo.Topic_SelectByCourseId";
        private const string _topicSelectAllProcedure = "dbo.Topic_SelectAll";
        private const string _topicUpdateProcedure = "dbo.Topic_Update";

        private const string _courseTopicInsertProcedure = "dbo.Course_Topic_Insert";
        private const string _courseTopicAddMultipleProcedure = "dbo.Course_Topic_AddMultiple";
        private const string _courseTopicDeleteProcedure = "dbo.Course_Topic_Delete";
        private const string _courseTopicSelectByIdProcedure = "dbo.Course_Topic_SelectById";
        private const string _courseTopicSelectBySeveralIdProcedure = "dbo.Course_Topic_SelectBySeveralId";
        private const string _courseTopicType = "dbo.Course_TopicType";
        private const string _idType = "dbo.IdType";

        private const string _tagTopicInsertProcedure = "dbo.Tag_Topic_Insert";
        private const string _tagTopicDeleteProcedure = "dbo.Tag_Topic_Delete";

        public TopicRepository(IOptions<DatabaseSettings> options) : base(options) { }

        public int AddTopic(TopicDto topicDto)
        {
            return _connection.QuerySingle<int>(
                _topicInsertProcedure,
                 new
                 {
                     topicDto.Name,
                     topicDto.Duration
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void DeleteTopic(int id)
        {
            _connection.Execute(
             _topicDeleteProcedure,
             new { id },
             commandType: CommandType.StoredProcedure
         );
        }

        public TopicDto GetTopic(int id)
        {
            return _connection.QuerySingleOrDefault<TopicDto>(
              _topicSelectByIdProcedure,
              new { id },
              commandType: CommandType.StoredProcedure
          );
        }

        public List<TopicDto> GetAllTopics()
        {
            return _connection
              .Query<TopicDto>(
                  _topicSelectAllProcedure,
                  commandType: CommandType.StoredProcedure
              )
              .AsList();
        }

        public int UpdateTopic(TopicDto topicDto)
        {
            return _connection.Execute(
                 _topicUpdateProcedure,
                 new
                 {
                     topicDto.Id,
                     topicDto.Name,
                     topicDto.Duration
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public int AddTopicToCourse(CourseTopicDto dto)
        {
            return _connection.QuerySingle<int>(
                _courseTopicInsertProcedure,
                new { CourseId = dto.Course.Id, TopicId = dto.Topic.Id, dto.Position },
                commandType: CommandType.StoredProcedure
                );
        }

        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            _connection.Execute(
                _courseTopicDeleteProcedure,
                new { courseId, topicId },
                commandType: CommandType.StoredProcedure
                );
        }

        public int AddTagToTopic(int topicId, int tagId)
        {
            return _connection.Execute(
                _tagTopicInsertProcedure,
                new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int DeleteTagFromTopic(int topicId, int tagId)
        {
            return _connection.Execute(
                _tagTopicDeleteProcedure,
                new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<int> AddTopicsToCourse(List<CourseTopicDto> dto)
        {
            var dt = new DataTable();
            dt.Columns.Add("CourseId");
            dt.Columns.Add("TopicId");
            dt.Columns.Add("Position");

            foreach (var topic in dto)
            {
                dt.Rows.Add(topic.Course.Id, topic.Topic.Id, topic.Position);
            }
            return _connection.Query<int>(
                _courseTopicAddMultipleProcedure,
                new { tblCourseTopic = dt.AsTableValuedParameter(_courseTopicType) },
                commandType: CommandType.StoredProcedure
                ).ToList();
        }

        public List<TopicDto> GetTopicsByCourseId(int courseId)
        {
            return _connection
                .Query<TopicDto>(
                    _topicSelectByCourseIdProcedure,
                    new { courseId },
                    commandType: CommandType.StoredProcedure
                ).
                ToList();
        }

        public CourseTopicDto GetCourseTopicById(int id)
        {
            var response = _connection.Query<CourseTopicDto, TopicDto, CourseTopicDto>(
                _courseTopicSelectByIdProcedure,
                (course, topic) =>
                {
                    course.Topic = topic;
                    return course;
                },
                new { id },
                splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ).FirstOrDefault();
            return response;
        }

        public List<CourseTopicDto> GetCourseTopicBySeveralId(List<int> ids)
        {
            var table = new DataTable();
            table.Columns.Add("Id");
            foreach (var i in ids)
            {
                table.Rows.Add(i);
            }
            var response = _connection.Query<CourseTopicDto, TopicDto, CourseTopicDto>(
                _courseTopicSelectBySeveralIdProcedure,
                (course, topic) =>
                {
                    course.Topic = topic;
                    return course;
                },
                new { @tblIds = table.AsTableValuedParameter(_idType) },
                splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            return response;
        }
    }
}