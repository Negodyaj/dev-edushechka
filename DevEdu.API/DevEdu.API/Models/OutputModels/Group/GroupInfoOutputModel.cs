using System.ComponentModel;

namespace DevEdu.API.Models
{
    public class GroupInfoOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}