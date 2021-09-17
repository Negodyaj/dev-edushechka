using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        LessonDto AddLesson(UserIdentityInfo userIdentity, LessonDto lessonDto, List<int> topicIds);
        void DeleteLesson(UserIdentityInfo userIdentity, int id);
        Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(UserIdentityInfo userIdentity, int id);
        List<LessonDto> SelectAllLessonsByTeacherId(int id);
        LessonDto SelectLessonWithCommentsById(UserIdentityInfo userIdentity, int id);
        LessonDto SelectLessonWithCommentsAndStudentsById(UserIdentityInfo userIdentity, int id);
        LessonDto UpdateLesson(UserIdentityInfo userIdentity, LessonDto lessonDto, int id);
        void DeleteTopicFromLesson(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
        StudentLessonDto AddStudentToLesson(int lessonId, int userId, UserIdentityInfo userIdentityInfo);
        void DeleteStudentFromLesson(int lessonId, int userId, UserIdentityInfo userIdentityInfo);
        StudentLessonDto UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        StudentLessonDto UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        StudentLessonDto UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId, UserIdentityInfo userIdentityInfo);
    }
}