using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DevEdu.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ITopicRepository _topicRepository;
        public CourseService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public void AddTopicToCourse(CourseTopicDto dto)
        {
            _topicRepository.AddTopicToCourse(dto);
        }
        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            _topicRepository.DeleteTopicFromCourse(courseId, topicId);
        }
    }
}
