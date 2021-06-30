using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class TopicInputModel
    {        
            [Required]
            public string Name { get; set; }
            public string Duration { get; set; }
       
    }
}
