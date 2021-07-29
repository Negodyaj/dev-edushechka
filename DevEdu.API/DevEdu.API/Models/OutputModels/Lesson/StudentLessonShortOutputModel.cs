namespace DevEdu.API.Models.OutputModels.Lesson
{
    public class StudentLessonShortOutputModel
    {
        public int Id { get; set; }
        public string Feedback { get; set; }
        public bool IsPresent { get; set; }
        public string AbsenceReason { get; set; }

    }
}
