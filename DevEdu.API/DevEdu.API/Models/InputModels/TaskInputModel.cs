using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class TaskInputModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate  { get; set; }
        [Required]
        public DateTime EndDate  { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Links { get; set; }
        [Required]
        public bool IsRequired { get; set; }
    }
}
