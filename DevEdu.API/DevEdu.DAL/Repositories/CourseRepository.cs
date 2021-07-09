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
        // todo: rename it
        private const string _insertProcedure = "dbo.Course_Insert";
        private const string _deleteProcedure = "dbo.Course_Delete";
        private const string _selectByIdProcedure = "dbo.Course_SelectById";
        private const string _selectAllProcedure = "dbo.Course_SelectAll";
        private const string _updateProcedure = "dbo.Course_Update";
        private const string _tagToTopicAddProcedure = "dbo.Tag_Topic_Insert";
        private const string _tagFromTopicDeleteProcedure = "dbo.Tag_Topic_Delete";

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
            CourseDto result = default;
            return _connection.Query<CourseDto, TopicDto, MaterialDto, TaskDto, GroupDto, CourseDto>(
                _selectByIdProcedure,
                (course, topic, material, task, group) =>
                {
                    if (result == null)
                    {
                        result = course;
                        result.Topics = new List<TopicDto> {topic};
                        result.Materials = new List<MaterialDto> {material};
                        result.Tasks = new List<TaskDto> {task};
                        result.Groups = new List<GroupDto> {group};
                    }
                    else
                    {
                        result.Topics.Add(topic);
                        result.Materials.Add(material);
                        result.Tasks.Add(task);
                        result.Groups.Add(group);
                    }
                    return course;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            ).FirstOrDefault();
            return result;
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
    }
}