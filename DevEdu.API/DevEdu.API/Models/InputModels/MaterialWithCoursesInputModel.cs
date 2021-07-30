using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class MaterialWithCoursesInputModel : MaterialInputModel
    {
        [MinLength(1, ErrorMessage = CoursesRequired)]
        public List<int> CoursesIds { get; set; }
    }
}
