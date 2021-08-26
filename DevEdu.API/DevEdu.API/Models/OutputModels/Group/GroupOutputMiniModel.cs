using DevEdu.DAL.Enums;

namespace DevEdu.API.Models
{
    public class GroupOutputMiniModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public string StartDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}