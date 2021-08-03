using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        void AddCommentToLesson(int lessonId, CommentDto commentDto);
        LessonDto AddLesson(UserDto userIdentity, LessonDto lessonDto, List<int> topicIds);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(UserDto userIdentity, int id);
        List<LessonDto> SelectAllLessonsByGroupId(UserDto userIdentity, int id);
        List<LessonDto> SelectAllLessonsByTeacherId(int id);
        LessonDto SelectLessonWithCommentsById(UserDto userIdentity, int id);
        LessonDto SelectLessonWithCommentsAndStudentsById(UserDto userIdentity, int id);
        LessonDto UpdateLesson(UserDto userIdentity, LessonDto lessonDto, int id);
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