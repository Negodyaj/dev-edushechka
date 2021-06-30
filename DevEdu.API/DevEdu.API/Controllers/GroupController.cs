using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        public GroupController()
        {

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
