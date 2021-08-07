using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DevEdu.DAL.Enums;

namespace DevEdu.API.Extensions
{
    public static class BaseControllerExtensions
    {
        public static int GetUserId(this ControllerBase controller)
        {
            var userId = Convert.ToInt32(controller.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return userId;
        }

        public static List<Role> GetUserRoles(this Controller controller)
        {
            return controller.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => (Role)Enum.Parse(typeof(Role), c.Value)).ToList();
        }

        public static UserIdentityInfo GetUserIdAndRoles(this Controller controller)
        {
            var userId = Convert.ToInt32(controller.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var roles = controller.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => (Role)Enum.Parse(typeof(Role), c.Value)).ToList();
            return new UserIdentityInfo
            {
                UserId = userId,
                Roles = roles
            };
        }
    }
}