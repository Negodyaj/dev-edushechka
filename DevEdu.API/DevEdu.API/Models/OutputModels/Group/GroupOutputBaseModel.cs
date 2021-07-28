using DevEdu.DAL.Enums;

namespace DevEdu.API.Models.OutputModels
{
    public class GroupOutputBaseModel
    {
        public int Id { get; set; }
        public GroupStatus GroupStatus { get; set; }
    }
}