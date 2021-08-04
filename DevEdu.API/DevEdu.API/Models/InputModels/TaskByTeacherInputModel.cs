using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class TaskByTeacherInputModel : TaskInputModel
    {
        public GroupTaskInputModel GroupTask { get; set; }
        public int GroupId { get; set; }
    }
}