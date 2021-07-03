﻿using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ICourseRepository
    {
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        CourseDto GetCourse(int id);
        List<CourseDto> GetCourses();
        void UpdateCourse(int id, CourseDto courseDto);
    }
}