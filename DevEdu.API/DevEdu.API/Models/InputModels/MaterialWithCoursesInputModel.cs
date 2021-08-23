using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class MaterialWithCoursesInputModel : MaterialWithTagsInputModel
    {
        [MinLength(1, ErrorMessage = CoursesRequired)]
        public List<int> CoursesIds { get; set; }
    }
}