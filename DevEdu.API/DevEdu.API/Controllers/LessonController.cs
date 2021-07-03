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
    }
}