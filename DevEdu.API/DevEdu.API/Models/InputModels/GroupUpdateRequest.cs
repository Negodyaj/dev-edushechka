using DevEdu.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class GroupUpdateRequest : GroupInputModel
    {
        [Required(ErrorMessage = GroupStatusIdRequired)]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = WrongFormatGroupStatusId)]
        public GroupStatus? GroupStatusId { get; set; }
    }
}