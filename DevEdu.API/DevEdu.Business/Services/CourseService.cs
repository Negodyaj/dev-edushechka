﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void AddTagToTopic(int topicId, int tagId) => _courseRepository.AddTagToTopic(topicId, tagId);

        public void DeleteTagFromTopic(int topicId, int tagId) => _courseRepository.DeleteTagFromTopic(topicId, tagId);
    }
}