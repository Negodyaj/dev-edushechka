using System.Collections.Generic;

namespace DevEdu.API.Models.InputModels
{
    public class TaskByMethodistInputModel : TaskInputModel
    {
        public List<int> CourseIds { get; set; }
    }
}