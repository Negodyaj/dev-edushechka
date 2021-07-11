using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevEdu.API.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class CourseTopicInputModel
    {
        [Required(ErrorMessage = ValidationMessage.PositionRequired)]
        public int Position { get; set; }

    }
}
