using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.API.Models.InputModels;

namespace DevEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        public GroupController()
        {
        }

        //  api/Group/5
        [HttpGet("{id}")]
        public string GetGroup(int id)
        {
            return $"Group №{id}";
        }

        //  api/Group/all
        [HttpGet("all")]
        public string GetAllGroup()
        {
            return "All Group";
        }

        //  api/Group
        [HttpPost]
        public int AddGroup([FromBody] GroupInputModel model)
        {
            return 1;
        }

        //  api/Group
        [HttpDelete]
        public void DeleteGroup(int id)
        {

        }

        ////  api/Group
        //[HttpPut]
        //public string UpdateGroup(int id, int courseId, int groupStatusId, DateTime startDate, string timetable, decimal paymentPerMonth)
        //{
        //    return $"Group №{id} change courseId to {courseId} and groupStatusId to {groupStatusId} and startDate to {startDate}" +
        //           $"and timetable to {timetable} and paymentPerMonth {paymentPerMonth}";
        //}

        //  api/Group
        [HttpPut]
        public string UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            return $"Group №{id} change courseId to {model.CourseId} and groupStatusId to {model.GroupStatusId} and startDate to {model.StartDate}" +
                   $"and timetable to {model.Timetable} and paymentPerMonth {model.PaymentPerMonth}";
        }

        //add group_lesson relation
        [HttpPost("group-lesson/{lessonId}/{groupId}")]
        public string AddGroupLessonReference(int lessonId, int groupId)
        {
            return (lessonId + groupId).ToString();
        }
        
        [HttpDelete("group-lesson/{lessonId}/{groupId}")]
        public string RemoveGroupLessonReference(int lessonId, int groupId)
        {
            return (lessonId - groupId).ToString();
        }

        [HttpPost("group-material/{materialId}/{groupId}")]
        public string AddGroupMaterialIdReference(int materialId, int groupId)
        {
            return (materialId + groupId).ToString();
        }

        [HttpDelete("group-material/{materialId}/{groupId}")]
        public string RemoveGroupMaterialIdReference(int materialId, int groupId)
        {
            return (materialId - groupId).ToString();
        }
    }
}