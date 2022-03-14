using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class LessonUpdateInputModel
    {
        [Required(ErrorMessage = LinkToRecordIdRequired)]
        [Url]
        public string LinkToRecord { get; set; }

        [Required(ErrorMessage = DateRequired)]
        public string Date { get; set; }

        public List<int> TopicIds { get; set; }
    }
}