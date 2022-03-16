using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class MaterialWithCoursesInputModel : MaterialInputModel
    {
        [Required(ErrorMessage = GroupsRequired)]
        [MinLength(1, ErrorMessage = CoursesRequired)]
        public List<int> CoursesIds { get; set; }
    }
}