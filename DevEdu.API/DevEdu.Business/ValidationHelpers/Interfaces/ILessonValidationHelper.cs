using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        void CheckLessonExistence(int lessonId);
        void CheckTeacherExistence(int teacherId);
        void CheckUserAndTeacherAreSame(UserDto userIdentity, int teacherId);
        void CheckUserBelongsToLesson(UserDto userIdentity, int lessonId);
    }
}