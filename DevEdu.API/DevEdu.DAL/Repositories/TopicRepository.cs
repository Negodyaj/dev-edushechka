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
    public class TopicRepository : BaseRepository
    {
        public int AddTopicToCourse(CourseTopicDto dto)
        {
            return _connection.QuerySingle<int>(
                "[dbo].[Course_Topic_Insert]",
                new { CourseId = dto.Course.Id, TopicId = dto.Topic.Id, dto.Position},
                commandType: CommandType.StoredProcedure
                );
        }
        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            _connection.Execute(
                "[dbo].[Course_Topic_Delete]",
                new { courseId, topicId },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
