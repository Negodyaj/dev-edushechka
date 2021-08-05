using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        LessonDto CheckLessonExistence(int lessonId);
        void CheckUserAndTeacherAreSame(UserDto userIdentity, int teacherId);
        void CheckUserBelongsToLesson(UserDto userIdentity, LessonDto lesson);
    }
}