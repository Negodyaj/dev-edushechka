using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        public LessonController()
        {

        }

        // api/lesson
        [HttpPost]
        public string AddLesson([FromBody] LessonInputModel inputModel)
        {
            return $"Date {inputModel.Date} TeacherComment {inputModel.TeacherComment}  TeacherId {inputModel.TeacherId}";
        }

        // api/lesson/{id}
        [HttpDelete("{id}")]
        public string DeleteLesson(int id)
        {
            return $"id {id}";
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpPost("{lessonId}/comment/{commentId}")]
        public string AddLessonComment(int lessonId, int commentId)
        {
            return $"lessonId {lessonId} commentId {commentId}";
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpDelete("{lessonId}/comment/{commentId}")]
        public string DeleteLessonComment(int lessonId, int commentId)
        {
            return $"lessonId {lessonId} commentId {commentId}";
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        public string DeleteTopicFromLesson(int lessonId, int topicId)
        {
            return $"lessonId {lessonId} topicId {topicId}";
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        public string AddTopicToLesson(int lessonId, int topicId)
        {
            return $"lessonId {lessonId} topicId {topicId}";
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpPost("{lessonId}/user/{userId}")]
        public string AddStudenToLesson(int userId, int lessonId)
        {
            return $"userId {userId} lessonId {lessonId} ";
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpDelete("{lessonId}/user/{userId}")]
        public string DeleteStudentFromLesson(int userId, int lessonId)
        {
            return $"userId {userId} lessonId {lessonId} ";
        }

        // api/lesson/{lessonId}/user/{userId}/feedback
        [HttpPut("{lessonId}/user/{userId}/feedback")]
        public string UpdateStudentFeedbackForLesson(int userId, int lessonId, [FromBody] FeedbackInputModel inputModel)
        {
            return $"userId {userId} lessonId {lessonId}, feedback {inputModel.Feedback}  ";
        }

        // api/lesson/{lessonId}/user/{userId}/absenceReason
        [HttpPut("{lessonId}/user/{userId}/absenceReason ")]
        public string UpdateStudentAbsenceReasonOnLesson(int userId, int lessonId, [FromBody] AbsenceReasonInputModel inputModel)
        {
            return $"userId {userId} lessonId {lessonId},absenceReason {inputModel.AbsenceReason}  ";
        }

        // api/lesson/{lessonId}/user/{userId}/attendance
        [HttpPut("{lessonId}/user/{userId}/attendance ")] 
        public string UpdateStudentAttendanceOnLesson(int userId, int lessonId, [FromBody] AttendanceInputModel inputModel)
        {
            return $"userId {userId} lessonId {lessonId},isPresent {inputModel.IsPresent} ";
        }
    }
}