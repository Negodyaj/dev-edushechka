using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Repositories;

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
        public string GetGroupById(int id)
        {
            return $"Group №{id}";
        }

        //  api/Group/
        [HttpGet]
        public string GetAllGroups()
        {
            return "All Groups";
        }

        //  api/Group
        [HttpPost]
        public int AddGroup([FromBody] GroupInputModel model)
        {
            return 1;
        }

        //  api/Group
        [HttpDelete("{id}")]
        public void DeleteGroup(int id)
        {

        }

        //  api/Group
        [HttpPut]
        public string UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            return $"Group №{id} change courseId to {model.CourseId} and timetable to {model.Timetable} and startDate to {model.StartDate}" +
                   $"and paymentPerMonth {model.PaymentPerMonth}";
        }

        //  api/Group/{groupId}/change-status/{statusId}
        [HttpPut("{groupId}/change-status/{statusId}")]
        public void ChangeGroupStatus(int groupId, int statusId)
        {

        }

        //add group_lesson relation
        // api/Group/{groupId}/lesson/{lessonId}
        [HttpPost("{groupId}/lesson/{lessonId}")]
        public void AddGroupLessonReference(int groupId, int lessonId)
        {
             GroupRepository.AddGroupLesson(groupId,  lessonId);
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        public void RemoveGroupLessonReference(int groupId, int lessonId)
        {
            GroupRepository.RemoveGroupLesson(groupId, lessonId);
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpPost("{groupId}/material/{materialId}")]
        public void AddGroupMaterialReference(int groupId, int materialId)
        {
            GroupRepository.AddGroupMaterial(groupId, materialId);
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpDelete("{groupId}/material/{materialId}")]
        public void RemoveGroupMaterialIdReference(int groupId, int materialId)
        {
            GroupRepository.RemoveGroupMaterial(groupId, materialId);
        }
    }
}