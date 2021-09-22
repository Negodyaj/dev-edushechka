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

        public async Task<int> AddTopicAsync(TopicDto topicDto)
        {
            return await _connection.QuerySingleAsync<int>(
                _topicInsertProcedure,
                 new
                 {
                     topicDto.Name,
                     topicDto.Duration
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task DeleteTopicAsync(int id)
        {
            await _connection.ExecuteAsync(
             _topicDeleteProcedure,
             new { id },
             commandType: CommandType.StoredProcedure
         );
        }

        public async Task<TopicDto> GetTopicAsync(int id)
        {
            return await _connection.QuerySingleOrDefaultAsync<TopicDto>(
              _topicSelectByIdProcedure,
              new { id },
              commandType: CommandType.StoredProcedure
          );
        }

        public async Task<List<TopicDto>> GetAllTopicsAsync()
        {
            return (await _connection
              .QueryAsync<TopicDto>(
                  _topicSelectAllProcedure,
                  commandType: CommandType.StoredProcedure
              ))
              .AsList();
        }

        public async Task<int> UpdateTopicAsync(TopicDto topicDto)
        {
            return await _connection.ExecuteAsync(
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

        public async Task<int> AddTopicToCourseAsync(CourseTopicDto dto)
        {
            return await _connection.QuerySingleAsync<int>(
                _courseTopicInsertProcedure,
                new { CourseId = dto.Course.Id, TopicId = dto.Topic.Id, dto.Position },
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task DeleteTopicFromCourseAsync(int courseId, int topicId)
        {
            await _connection.ExecuteAsync(
                 _courseTopicDeleteProcedure,
                 new { courseId, topicId },
                 commandType: CommandType.StoredProcedure
                 );
        }

        public async Task<int> AddTagToTopicAsync(int topicId, int tagId)
        {
            return await _connection.ExecuteAsync(
                _tagTopicInsertProcedure,
                new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteTagFromTopicAsync(int topicId, int tagId)
        {
            return await _connection.ExecuteAsync(
                _tagTopicDeleteProcedure,
                new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<int>> AddTopicsToCourseAsync(List<CourseTopicDto> dto)
        {
            var dt = new DataTable();
            dt.Columns.Add("CourseId");
            dt.Columns.Add("TopicId");
            dt.Columns.Add("Position");

            foreach (var topic in dto)
            {
                dt.Rows.Add(topic.Course.Id, topic.Topic.Id, topic.Position);
            }
            return (await _connection.QueryAsync<int>(
                _courseTopicAddMultipleProcedure,
                new { tblCourseTopic = dt.AsTableValuedParameter(_courseTopicType) },
                commandType: CommandType.StoredProcedure
                )).ToList();
        }

        public async Task<List<TopicDto>> GetTopicsByCourseIdAsync(int courseId)
        {
            return (await _connection
                .QueryAsync<TopicDto>(
                    _topicSelectByCourseIdProcedure,
                    new { courseId },
                    commandType: CommandType.StoredProcedure
                )).ToList();
        }

        public async Task<CourseTopicDto> GetCourseTopicByIdAsync(int id)
        {
            var response = (await _connection.QueryAsync<CourseTopicDto, TopicDto, CourseTopicDto>(
                _courseTopicSelectByIdProcedure,
                (course, topic) =>
                {
                    course.Topic = topic;
                    return course;
                },
                new { id },
                splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )).FirstOrDefault();

            return response;
        }

        public async Task<List<CourseTopicDto>> GetCourseTopicBySeveralIdAsync(List<int> ids)
        {
            var table = new DataTable();
            table.Columns.Add("Id");

            foreach (var i in ids)
            {
                table.Rows.Add(i);
            }

            var response = (await _connection.QueryAsync<CourseTopicDto, TopicDto, CourseTopicDto>(
                _courseTopicSelectBySeveralIdProcedure,
                (course, topic) =>
                {
                    course.Topic = topic;
                    return course;
                },
                new { @tblIds = table.AsTableValuedParameter(_idType) },
                splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )).ToList();

            return response;
        }
    }
}