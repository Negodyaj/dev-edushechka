using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class TopicRepository : BaseRepository, ITopicRepository
    {
        private const string _addTopicToCourseProcedure = "[dbo].[Course_Topic_Insert]";
        private const string _deleteTopicToCourseProcedure = "[dbo].[Course_Topic_Delete]";
        public int AddTopicToCourse(CourseTopicDto dto)
        {
            return _connection.QuerySingle<int>(
                _addTopicToCourseProcedure,
                new { CourseId = dto.Course.Id, TopicId = dto.Topic.Id, dto.Position },
                commandType: CommandType.StoredProcedure
                );
        }
        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            _connection.Execute(
                _deleteTopicToCourseProcedure,
                new { courseId, topicId },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
