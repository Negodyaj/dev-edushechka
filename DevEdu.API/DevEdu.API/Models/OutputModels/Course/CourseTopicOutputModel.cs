namespace DevEdu.API.Models.OutputModels
{
    public class CourseTopicOutputModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public TopicOutputModel Topic { get; set; }
    }
}