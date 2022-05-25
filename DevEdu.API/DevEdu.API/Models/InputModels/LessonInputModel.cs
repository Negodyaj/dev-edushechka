using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class LessonInputModel
    {
        [Required(ErrorMessage = DateRequired)]
        public string Date { get; set; }

        [Required(ErrorMessage = AdditionalMaterialsRequired)]
        public string AdditionalMaterials { get; set; }

        [Required(ErrorMessage = GroupIdRequired)]
        public int GroupId { get; set; }

        public string Name { get; set; }

        [Url] public string LinkToRecord { get; set; }

        public List<int> TopicIds { get; set; }
        public bool IsPublished { get; set; }
    }
}