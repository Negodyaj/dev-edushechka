﻿using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        LessonDto GetLessonByIdAndThrowIfNotFound(int lessonId);
        public void CheckTopicLessonReferenceIsUnique(LessonDto lesson, int topicId);    
        void CheckUserAndTeacherAreSame(UserIdentityInfo userIdentity, int teacherId);
        void CheckUserBelongsToLesson(UserIdentityInfo userIdentity, LessonDto lesson);
        void CheckUserInLessonAccess(int lessonId, int userId);
    }
}