using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Extensions
{
    public static class ControllerExtensions
    {
        public static int GetUserId(this ControllerBase controller)
        {
            var userId =  Convert.ToInt32(controller.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return userId;
        }
        public static List<Role> GetUserRoles(this ControllerBase controller)
        {
            return controller.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => (Role)Enum.Parse(typeof(Role), c.Value)).ToList();
        }
    }
}
