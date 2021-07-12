using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class TopicInputModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Duration { get; set; }

    }
}