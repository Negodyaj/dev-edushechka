using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class User_GroupController : Controller
    {
        private User_GroupRepository _repository;
        public User_GroupController(User_GroupRepository repository)
        {
            _repository = repository;
        }

        //  api/user_group/group/1/user/2/role/1
        [HttpPost("group/{groupid}/user/{userid}/role/{roleid}")]
        public void AddUser_Group(int groupId, int userId, int roleId)
        {
            _repository.AddTag(groupId, userId, roleId);
        }

        //  api/user_group/group/1/user/2
        [HttpDelete("group/{groupid}/user/{userid}")]
        public void DeleteUser_Group(int groupId, int userId)
        {
            _repository.DeleteTag(userId, groupId);
        }
    }
}
