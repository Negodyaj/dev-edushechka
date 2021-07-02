using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using DevEdu.DAL.Repositories;
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
        private LessonRepository _repository;
        public LessonController()
        {
            _repository = new LessonRepository();
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
        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _repository.DeleteTopicFromLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _repository.AddTopicToLesson(lessonId, topicId);
        }




        // api/lesson/userId/{userId}/lessonId/{lessonId} 
        [HttpPost("userId/{userId}/lessonId/{lessonId} ")]
        public string AddStudenToLesson(int userId, int lessonId)
        {
            return $"userId {userId} lessonId {lessonId} ";
        }


           // api/lesson/userId/{userId}/lessonId/{lessonId} 
        [HttpDelete("userId/{userId}/lessonId/{lessonId} ")]
        public string DeleteStodentFromLesson(int userId, int lessonId)
        {
            return $"userId {userId} lessonId {lessonId} ";
        }


        // api/lesson/userId/{userId}/lessonId/{lessonId}/feedback 
        [HttpPut("userId/{userId}/lessonId/{lessonId}/feedback ")]
        public string UpdateFeedbackOfStudenLesson(int userId, int lessonId, [FromBody] StudentLessonUpdateFeedbackInputModel inputModel)
        {
            return $"userId {userId} lessonId {lessonId}, feedback {inputModel.Feedback}  ";
        }

        // api/lesson/userId/{userId}/lessonId/{lessonId}/absenceReason
        [HttpPut("userId/{userId}/lessonId/{lessonId}/absenceReason ")]
        public string UpdateAbsenceReasonOfStudenLesson(int userId, int lessonId, [FromBody] StudentLessonUpdateAbsenceReasonInputModel inputModel)
        {
            return $"userId {userId} lessonId {lessonId},absenceReason {inputModel.AbsenceReason}  ";
        }

        // api/lesson/userId/{userId}/lessonId/{lessonId}/isPresent
        [HttpPut("userId/{userId}/lessonId/{lessonId}/isPresent ")] 
        public string UpdateIsPresentOfStudenLesson(int userId, int lessonId, [FromBody] StudentLessonUpdateIsPresentInputModel inputModel)
        {
            return $"userId {userId} lessonId {lessonId},isPresent {inputModel.IsPresent} ";
        }
    }
}
