using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        public LessonController()
        {

        }

        [HttpPost("lesson/{date}/{teacherComment}/{teacherId}")]
        public string AddLesson(DateTime date, String teacherComment, int teacherId)
        {
            return $"Date {date} TeacherComment {teacherComment}  TeacherId {teacherId}";
        }

        [HttpDelete("lesson/{id}")]
        public string DeleteLesson(int id)
        {
            return $"id {id}";
        }

        [HttpPost("lesson-comment/{date}/{teacherComment}/{teacherId}")]
        public string AddLessonComment(int lessonId, int commentId)
        {
            return $"lessonId {lessonId} commentId {commentId}";
        }

        [HttpDelete("lesson-comment/{id}")]
        public string DeleteLessonComment(int lessonId, int commentId)
        {
            return $"lessonId {lessonId} commentId {commentId}";
        }

        [HttpDelete("lesson/{lessonId}/topic/{toppicId}")]
        public string DeleteTopicFromLesson(int lessonId, int toppicId)
        {
            return $"lessonId {lessonId} topicId {toppicId}";
        }

        [HttpPost("lesson/{lessonId}/topic/{toppicId}")]
        public string AddTopicToLesson(int lessonId, int toppicId)
        {
            return $"lessonId {lessonId} topicId {toppicId}";
        }
    }
}
