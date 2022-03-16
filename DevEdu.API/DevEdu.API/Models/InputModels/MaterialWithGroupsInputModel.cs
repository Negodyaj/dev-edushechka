using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class MaterialWithGroupsInputModel : MaterialInputModel
    {
        [Required(ErrorMessage = GroupsRequired)]
        [MinLength(1, ErrorMessage = GroupsRequired)]
        public List<int> GroupsIds { get; set; }
    }
}