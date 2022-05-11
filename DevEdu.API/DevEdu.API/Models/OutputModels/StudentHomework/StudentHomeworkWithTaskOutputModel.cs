using DevEdu.DAL.Enums;
using System.Threading.Tasks;

namespace DevEdu.API.Models
{
    public class StudentHomeworkWithTaskOutputModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public string CompletedDate { get; set; }
        public StudentHomeworkStatus Status { get; set; }
        public HomeworkInfoWithTaskOutputModel Homework { get; set; }
        public bool IsDeleted { get; set; }
    }
}