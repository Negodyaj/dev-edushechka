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
        public static int GetUserId(this Controller i)
        {
            var userId =  Convert.ToInt32(i.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return userId;
        }
        public static List<Role> GetUserRoles(this Controller i)
        {
            return i.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => (Role)Enum.Parse(typeof(Role), c.Value)).ToList();
        }
    }
}
