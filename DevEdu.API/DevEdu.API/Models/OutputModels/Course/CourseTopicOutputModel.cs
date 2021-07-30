namespace DevEdu.API.Models
{
    public class CourseTopicOutputModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public TopicOutputModel Topic { get; set; }
        public bool IsDeleted { get; set; }
    }
}