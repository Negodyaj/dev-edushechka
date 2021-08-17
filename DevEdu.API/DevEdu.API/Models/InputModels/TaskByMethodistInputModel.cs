using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class TaskByMethodistInputModel : TaskInputModel
    {
        public List<int> CourseIds { get; set; }
    }
}