using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class TopicInputModel
    {            
            [Required(ErrorMessage = NameRequired)]             
            public string Name { get; set; }         
            [Required(ErrorMessage = DurationRequired)]             
            public int Duration { get; set; }
    }
}