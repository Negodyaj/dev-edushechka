using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class AttendanceInputModel
    {
        [Required]
         public string IsPresent { get; set; }

    }
}
