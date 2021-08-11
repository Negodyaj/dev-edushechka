using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        LessonDto AddLesson(UserIdentityInfo userIdentity, LessonDto lessonDto, List<int> topicIds);
        void DeleteLesson(UserIdentityInfo userIdentity, int id);
        List<LessonDto> SelectAllLessonsByGroupId(UserIdentityInfo userIdentity, int id);
        List<LessonDto> SelectAllLessonsByTeacherId(int id);
        LessonDto SelectLessonWithCommentsById(UserIdentityInfo userIdentity, int id);
        LessonDto SelectLessonWithCommentsAndStudentsById(UserIdentityInfo userIdentity, int id);
        LessonDto UpdateLesson(UserIdentityInfo userIdentity, LessonDto lessonDto, int id);
        void DeleteTopicFromLesson(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
        StudentLessonDto AddStudentToLesson(int lessonId, int userId);
        void DeleteStudentFromLesson(int lessonId, int userId);
        StudentLessonDto UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        StudentLessonDto UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        StudentLessonDto UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId);
    }
}