using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class TagUpdateInputModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
